using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Forms_CORE_CHAT_UGH.Models;


//https://www.youtube.com/watch?v=xgLRe7QV6QI&ab_channel=HazardEditHazardEdit
namespace Windows_Forms_Chat
{
    public partial class Form1 : Form
    {
        TicTacToe ticTacToe = new TicTacToe();
        TCPChatServer server = null;
        TCPChatClient client = null;
        List<User> Users = new List<User>();

        public Form1()
        {
            InitializeComponent();

        }

        public bool CanHostOrJoin()
        {
            if (server == null && client == null)
                return true;
            else
                return false;
        }

        private void HostButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    server = TCPChatServer.createInstance(port, ChatTextBox, listUserOnline);
                    //oh no, errors
                    if (server == null)
                        throw new Exception("Incorrect port value!");//thrown exceptions should exit the try and land in next catch

                    var user = new User()
                    {
                        Id = Users.Count + 1,
                        Username = "admin",
                        Role = "admin"

                    };
                    Users.Add(user);
                    server.SetupServer();
                    IsAdmin(user.Username);

                }
                catch (Exception ex)
                {
                    ChatTextBox.Text += "Error: " + ex;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }

        }

        private void JoinButton_Click(object sender, EventArgs e)
        {

            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    int serverPort = int.Parse(serverPortTextBox.Text);
                    client = TCPChatClient.CreateInstance(port, serverPort, ServerIPTextBox.Text, ChatTextBox, listUserOnline);

                    if (client == null)
                        throw new Exception("Incorrect port value!");//thrown exceptions should exit the try and land in next catch

                    //check username exists listUseronline

                    var user = new User()
                    {
                        Id = Users.Count + 1,
                        Username = txtUsername.Text,
                        Role = "member"
                    };
                    if (Users.Any(x => x.Username == user.Username))
                    {
                        MessageBox.Show("Username already exists");
                        return;
                    }
                    User userConnect = client.ConnectToServer(user);
                    client.SendString($" your connected");
                    // var usersOnline = server.UserOnline();
                    Users.Add(userConnect);
                }
                catch (Exception ex)
                {
                    client = null;
                    ChatTextBox.Text += "Error: " + ex;
                    ChatTextBox.AppendText(Environment.NewLine);
                }

            }
        }
        private void IsAdmin(string username)
        {

            var admin = Users.Find(x => x.Role == username && x.Username == username);
            if (admin.Username == "admin")
            {

                btnKick.Visible = true;
                lbUsernameAdmin.Text = admin.Username;
            }

        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            if (client != null)
                client.SendString(TypeTextBox.Text, username);
            else if (server != null)
                server.SendToAll(TypeTextBox.Text, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnKick.Visible = false;
            //On form loaded
            ticTacToe.buttons.Add(button1);
            ticTacToe.buttons.Add(button2);
            ticTacToe.buttons.Add(button3);
            ticTacToe.buttons.Add(button4);
            ticTacToe.buttons.Add(button5);
            ticTacToe.buttons.Add(button6);
            ticTacToe.buttons.Add(button7);
            ticTacToe.buttons.Add(button8);
            ticTacToe.buttons.Add(button9);

            JoinButton.Enabled = false;
        }

        private void AttemptMove(int i)
        {
            if (ticTacToe.myTurn)
            {
                bool validMove = ticTacToe.SetTile(i, ticTacToe.playerTileType);
                if (validMove)
                {
                    //tell server about it
                    //ticTacToe.myTurn = false;//call this too when ready with server
                }
                //example, do something similar from server
                GameState gs = ticTacToe.GetGameState();
                if (gs == GameState.crossWins)
                {
                    ChatTextBox.AppendText("X wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.naughtWins)
                {
                    ChatTextBox.AppendText(") wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.draw)
                {
                    ChatTextBox.AppendText("Draw!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AttemptMove(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttemptMove(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AttemptMove(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AttemptMove(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AttemptMove(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AttemptMove(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AttemptMove(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AttemptMove(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AttemptMove(8);
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            JoinButton.Enabled = (txtUsername.Text.Length > 0);
        }
    }
}
