using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Windows_Forms_CORE_CHAT_UGH.Models;

namespace Windows_Forms_Chat
{

    public class TCPChatBase
    {
        public TextBox chatTextBox;
        public ListBox listBox;
        public int port;
        public void SetChat(string str)
        {
            chatTextBox.Invoke((Action)delegate
            {
                chatTextBox.Text = str;
                chatTextBox.AppendText(Environment.NewLine);
            });
        }
        public void SetListUser(string username)
        {
            listBox.Invoke((Action)delegate
            {
                listBox.Items.Add(username);
            });
        }
        public void RemoveListUser(string username)
        {
            listBox.Invoke((Action)delegate
            {
                listBox.Items.Remove(username);
            });
        }
        public void AddToChat(string str)
        {
            //dumb https://iandotnet.wordpress.com/tag/multithreading-how-to-update-textbox-on-gui-from-another-thread/
            chatTextBox.Invoke((Action)delegate
            {
                chatTextBox.AppendText(str);
                chatTextBox.AppendText(Environment.NewLine);
            });
        }
    }
}
