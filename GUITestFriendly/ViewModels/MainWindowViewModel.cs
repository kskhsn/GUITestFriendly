using GUITestFriendly.Models;
using Livet;
using Livet.Commands;

namespace GUITestFriendly.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        private Stringer Stringer = new Stringer();

        public void Initialize()
        {
        }
        public void ButtonClickCommand()
        {
            this.Answer = this.Stringer.Combine(this.Lhs, this.Rhs);
        }

        public void ButtonClickCommand2()
        {
            this.Answer = "!!!";
        }
        public void ButtonClickCommand3()
        {
            this.Answer = "???";
        }

        #region Lhs変更通知プロパティ
        private string _Lhs;

        public string Lhs
        {
            get { return this._Lhs; }

            set
            {
                if (this._Lhs == value)
                {
                    return;
                }

                this._Lhs = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region Rhs変更通知プロパティ
        private string _Rhs;

        public string Rhs
        {
            get { return this._Rhs; }

            set
            {
                if (this._Rhs == value)
                {
                    return;
                }

                this._Rhs = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region Answer変更通知プロパティ
        private string _Answer;

        public string Answer
        {
            get { return this._Answer; }

            set
            {
                if (this._Answer == value)
                {
                    return;
                }

                this._Answer = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region ACommand
        private ViewModelCommand _ACommand;

        public ViewModelCommand ACommand
        {
            get
            {
                if (_ACommand == null)
                {
                    _ACommand = new ViewModelCommand(A);
                }
                return _ACommand;
            }
        }

        public void A()
        {
            this.Answer = "666";
        }
        #endregion


        #region BCommand
        private ListenerCommand<string> _BCommand;

        public ListenerCommand<string> BCommand
        {
            get
            {
                if (_BCommand == null)
                {
                    _BCommand = new ListenerCommand<string>(B);
                }
                return _BCommand;
            }
        }

        public void B(string parameter)
        {
            this.Answer = new string(parameter[0], 3);
        }
        #endregion
    }
}
