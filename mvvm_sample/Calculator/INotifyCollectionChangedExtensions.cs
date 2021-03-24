using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Calculator
{
    static class INotifyCollectionChangeExtensions
    {
        /// <summary>
        /// senderオブジェクトへ観察対象のプロパティを指定してlistenerオブジェクトのメソッドを登録する
        /// </summary>
        /// <param name="p_propertyName">観察対象のプロパティ</param>
        /// <param name="p_handler">通知時の実行メソッド</param>
        public static IDisposable AddCollectionChanged<TObject>(
            this TObject p_self,
            Action p_handler)
            where TObject : INotifyCollectionChanged
        {
            // 追加/解除対象のイベントハンドラーを用意しておく
            // このオブジェクトを追加/解除時で、同じものを利用しないと
            // senderオブジェクトから解除できずにイベントハンドラーが残ってしまう
            NotifyCollectionChangedEventHandler l_handler =
                (sender, e) => p_handler.Invoke();

            // イベントハンドラーをsenderへ追加する
            p_self.CollectionChanged += l_handler;

            // listenerが破棄される際に、PropertyChangedのチェーンを確実に解除するために
            // イベントハンドラーをsenderから解除するメソッドを隠蔽化した破棄対象オブジェクトを生成して返す
            // Disposeはlistenerに任せる
            return new DelegateDisposable(() => p_self.CollectionChanged -= l_handler);
        }

        public static IDisposable InitializeSynced<TObject, TTarget, TSource>(
            this TObject p_self,
            ObservableCollection<TSource> p_items,
            Func<TSource, TTarget> p_getItem)
            where TObject : ObservableCollection<TTarget>
        {
            foreach (var l_c in p_items)
                p_self.Add(p_getItem.Invoke(l_c));

            var l_handler = new CollectionChangeHandler<TSource, TTarget>(
                p_self, p_getItem);

            p_items.CollectionChanged += l_handler.OnChildrenChanged;

            return new DelegateDisposable(
                () => p_items.CollectionChanged -= l_handler.OnChildrenChanged);
        }

        /// <summary>
        /// コレクションチェンジのハンドラを代行するクラス
        /// </summary>
        /// <typeparam name="TSource">更新元コレクションのアイテムタイプ</typeparam>
        /// <typeparam name="TTarget">更新先コレクションのアイテムタイプ</typeparam>
        private class CollectionChangeHandler<TSource, TTarget>
        {
            #region フィールド
            /// <summary>
            /// 更新先コレクション
            /// </summary>
            private readonly ObservableCollection<TTarget> items;

            /// <summary>
            /// 要素取得コールバック
            /// </summary>
            private readonly Func<TSource, TTarget> getItemCallBack;
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CollectionChangeHandler(
                ObservableCollection<TTarget> p_items,
                Func<TSource, TTarget> p_getItemCallBack)
            {
                this.items = p_items;
                this.getItemCallBack = p_getItemCallBack;
            }
            #endregion

            #region private メソッド
            /// <summary>
            /// コレクションチェンジハンドラ
            /// </summary>
            /// <param name="p_sender"></param>
            /// <param name="p_e"></param>
            public void OnChildrenChanged(object p_sender, NotifyCollectionChangedEventArgs p_e)
            {
                switch (p_e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        this.addExecute((TSource)p_e.NewItems[0], p_e.NewStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Move:
                        this.moveExecute(p_e.OldStartingIndex, p_e.NewStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        this.removeExecute(p_e.OldStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        this.replaceExecute((TSource)p_e.NewItems[0], p_e.NewStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        this.clearExecute();
                        break;
                }
            }

            /// <summary>
            /// 追加
            /// </summary>
            private void addExecute(TSource p_new, int p_index)
            {
                this.items.Insert(p_index, this.getNode(p_new));
            }

            /// <summary>
            /// 移動
            /// </summary>
            private void moveExecute(int p_oldIndex, int p_newIndex)
            {
                this.items.Move(p_oldIndex, p_newIndex);
            }

            /// <summary>
            /// 削除
            /// </summary>
            private void removeExecute(int p_oldIndex)
            {
                var l_removing = this.items.ElementAt(p_oldIndex);
                this.items.Remove(l_removing);
            }

            /// <summary>
            /// 置換
            /// </summary>
            private void replaceExecute(TSource p_new, int p_index)
            {
                this.items[p_index] = this.getNode(p_new);
            }

            /// <summary>
            /// クリア
            /// </summary>
            private void clearExecute()
            {
                this.items.Clear();
            }

            /// <summary>
            /// 新規子ノード取得
            /// </summary>
            private TTarget getNode(TSource p_newChildItem) => this.getItemCallBack.Invoke(p_newChildItem);
            #endregion
        }
    }
}
