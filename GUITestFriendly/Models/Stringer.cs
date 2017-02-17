namespace GUITestFriendly.Models
{
    public class Stringer 
    {
        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

        public string Combine(string str1, string str2)
        {
            return $"\"{str1}\" + \"{str2}\" = \"{str1 + str2}\";";
        }
    }
}
