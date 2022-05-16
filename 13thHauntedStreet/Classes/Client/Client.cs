﻿/*
 * Author : David Vieira Luis
 * Project : 13th Haunted Street
 * Details : Receives and sends data to the server
 */
// TODO traduire
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Text.RegularExpressions;

namespace _13thHauntedStreet
{
    public class Client
    {
        // Varaibles
        private TcpClient client = default(TcpClient);
        private Client clientLeader;
        private Game1 mainGame;
        private NetworkStream networkStream;
        private Socket sock;
        public static List<Player> dataPlayers = new List<Player>();
        public static List<Texture2D> listTextureOfPlayers = new List<Texture2D>();
        public static List<dataPlayer> dataOfPlayers = new List<dataPlayer>();
        public static List<foreignPlayer> listOtherPlayer = new List<foreignPlayer>();
        private dataPlayer dataOfMyPlayer = new dataPlayer();
        public static List<Vector2> listVectorOfPlayers = new List<Vector2>();
        public static Vector2 defaultPostion = new Vector2();
        public static Vector2 PositionPlayer = new Vector2();
        public static Texture2D texturePlayer;
        string nbJoueurPartie1;
        string nbJoueurPartie2;
        string dataOfServer;
        public bool objetJoueurExistant = false;

        private string _id = "rien";

        //permet de recevoir les messages en boucle
        public void refreshMessage()
        {
            while (true)
            {

                byte[] data = new byte[600];
                String responseData = string.Empty;
                if (networkStream.DataAvailable == true)
                {
                    Int32 bytes = networkStream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);


                    string test = responseData.Split(":")[0].Trim();
                    // permet de donner l'id au joueur ou sinon rajouter un espace vide dans la liste
                    if (responseData.Split(":")[0].Trim() == "Vous etes l'id" && _id == "rien")
                    {
                        _id = responseData.Split(":")[1].Trim();
                    }
                    if (responseData.Split(":")[0].Trim() == "Nombre de joueur")
                    {
                        nbJoueurPartie1 = responseData.Split(":")[1].Trim();
                        nbJoueurPartie2 = nbJoueurPartie1.Split("!")[0];
                        responseData = "";
                        dataOfMyPlayer.Id = Convert.ToInt32(nbJoueurPartie2);
                        Game1.player.id = Convert.ToInt32(nbJoueurPartie2);
                        Vector2 positionClient = new Vector2(10, 10);
                        Player player = new Ghost(Vector2.Zero, Game1.ghostAM); // TODO changer apres
                        defaultPostion.X = 10;
                        defaultPostion.Y = 10;
                        dataPlayers.Add(player);


                    }
                    if (responseData.Split(":")[0].Trim() == "Je me déconnecte")
                    {
                        Console.WriteLine("le joueur " + responseData.Split(":")[1] + " se déconnecte");
                    }

                    // cela permet de savoir si l'information donné est une info pour les joueurs
                    // !!!! problème. si on rajoute un joueur après que un se soit deconnecter cela ne rentre pas sa positions dans la liste
                    if (responseData.Split("$")[0].Trim() == "<List>") // CHECK!!!
                    {
                        if (responseData.Split("$")[1].Contains("|"))
                        {
                            dataOfServer = responseData.Split("$")[1];
                            dataOfServer = dataOfServer.Split("|")[0];
                        }
                        else
                        {
                            dataOfServer = responseData.Split("$")[1];
                        }

                        #region deserialization  de la liste
                        XmlSerializer serializer = new XmlSerializer(dataOfPlayers.GetType());
                        using (
                             StringReader sw = new StringReader(dataOfServer))
                        {
                            dataOfPlayers = (List<dataPlayer>)serializer.Deserialize(sw); // CHECK!!!
                            int count = 0;
                            if (listTextureOfPlayers.Count != dataOfPlayers.Count)
                            {
                                //On doit ajouter une valeur vide dans la liste ducoup je compte le nombre d'item dans la liste de player et je fais un for pour ajouter null pour chauqe item manquant sinon ca fait une erreur
                                for (int i = 0; i < dataOfPlayers.Count; i++)
                                {
                                    if (i >= listTextureOfPlayers.Count)
                                    {
                                        listTextureOfPlayers.Add(null);
                                        listVectorOfPlayers.Add(defaultPostion);
                                    }
                                }
                            }
                            // pour chaque donnée de joueur dans la liste  on les traites et on stock la texture dans une list et la position dans une autre liste
                            #region stockages des données des textures et des positions dans des listes pour chaque catégorie
                            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                            ci.NumberFormat.CurrencyDecimalSeparator = ",";
                            foreach (dataPlayer p in dataOfPlayers)
                            {
                                if (!(p is null)) { 
                                string y1 = p.Position.Split("Y:")[1];

                                string y2 = y1.Split("}")[0];
                                //y2.Replace(",", ".");
                                y2 = y2.Trim();
                                string x1 = p.Position.Split("X:")[1];
                                string x2 = x1.Split("Y")[0];
                                // x2.Replace(",", ".");
                                x2 = x2.Trim();
                                if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                                {
                                    y2 = y2.Replace(".", ",");
                                    x2 = x2.Replace(".", ",");

                                }
                                else
                                {
                                    y2 = y2.Replace(",", ".");
                                    x2 = x2.Replace(",", ".");
                                }
                                float Y = float.Parse(y2);

                                float X = float.Parse(x2);
                                PositionPlayer.Y = Y;
                                PositionPlayer.X = X;

                                // TODO changer 
                                //   if (p.Texture == "sprite/Ghost1")
                                // {

                                //listTextureOfPlayers[count] = Game1.player.texture;

                                int frame = p.AnimFrame == -1 ? 0 : p.AnimFrame;
                                if (p.PlayerType == typeof(Hunter).ToString())
                                {
                                    switch (p.AnimName)
                                    {
                                        case "idle_down":
                                            texturePlayer = Game1.hunterAM.idleDown[frame];
                                            break;

                                        case "idle_up":
                                            texturePlayer = Game1.hunterAM.idleUp[frame];
                                            break;

                                        case "idle_right":
                                            texturePlayer = Game1.hunterAM.idleRight[frame];
                                            break;

                                        case "idle_left":
                                            texturePlayer = Game1.hunterAM.idleLeft[frame];
                                            break;

                                        case "walking_down":
                                            texturePlayer = Game1.hunterAM.walkingDown[frame];
                                            break;

                                        case "walking_up":
                                            texturePlayer = Game1.hunterAM.walkingUp[frame];
                                            break;

                                        case "walking_right":
                                            texturePlayer = Game1.hunterAM.walkingRight[frame];
                                            break;

                                        case "walking_left":
                                            texturePlayer = Game1.hunterAM.walkingLeft[frame];
                                            break;
                                    }
                                }
                                else
                                {
                                        switch (p.AnimName)
                                        {
                                            case "left":
                                                texturePlayer = Game1.ghostAM.animationLeft[frame];
                                                break;

                                            case "right":
                                                texturePlayer = Game1.ghostAM.animationRight[frame];
                                                break;
                                        }
                                }
                                //texturePlayer = Game1.player.texture;
                                //}
                                /*if (p.Texture == "sprite/Ghost2")
                                {
                                    listTextureOfPlayers[count] = Game1.player2;
                                }*/
                                listVectorOfPlayers[count] = PositionPlayer;
                                // On regarde chaque joueur qu'il y a dans la list 
                                //  listOtherPlayer = new List<foreignPlayer>();
                                foreach (foreignPlayer fp in listOtherPlayer)
                                {
                                    //On regarde is l'id qu'on vient de recevoir et la même que l'une des idées qu'on a dans la liste.
                                    //Si c'est juste on lui met a jour ses valeurs et on dit qu'il existe
                                    if (p.Id == fp._id)
                                    {
                                        fp._Texture = texturePlayer;
                                        fp._Position = PositionPlayer;
                                        fp.playerType = p.PlayerType;
                                        objetJoueurExistant = true;
                                        break;
                                    }
                                }
                                //Si l'id n'existe pas dans la liste on le crée et ensuite on le rajoute dans la liste
                                if (objetJoueurExistant == false)
                                {
                                    foreignPlayer externPlayer = new foreignPlayer(PositionPlayer, texturePlayer, p.PlayerType);
                                    externPlayer._id = p.Id;
                                    listOtherPlayer.Add(externPlayer);
                                }
                                objetJoueurExistant = false;
                                #region useless

                                // mettre le code avec les commentaires en-dessous au-dessus
                                // avec le for parcours la listVectorOfPlayers et si l'id n'est pas dans la liste crée un objet
                                /*  int newId;
                                   for (int i = 0; i < listVectorOfPlayers.Count; i++)
                                   {
                                       foreach (foreignPlayer fp in listOtherPlayer)
                                       {
                                           if (fp._id == i)
                                           {
                                               fp._Texture = listTextureOfPlayers[count];
                                               fp._Position = listVectorOfPlayers[count];
                                               objetJoueurExistant = true;
                                           }
                                         //  newId = fp._id;

                                       }
                                       if (objetJoueurExistant == false)
                                       {
                                           foreignPlayer externPlayer = new foreignPlayer(listVectorOfPlayers[i], listTextureOfPlayers[i]);
                                           externPlayer._id = i;
                                           listOtherPlayer.Add(externPlayer);
                                       }
                                   } 
                               */
                                #endregion
                                count = count + 1;
                                }
                            }
                            count = 0;


                            #endregion
                        }

                        // !!!!!!!!! problème il faut réussir avec les liste de dire qu'elle donnée appartient à qui
                        for (int i = listOtherPlayer.Count; i > 1; i--)
                        {


                            bool existant = false;
                            foreach (dataPlayer p in dataOfPlayers)
                            {
                                if (listOtherPlayer[i - 1]._id == p.Id)
                                {
                                    existant = true;
                                }

                            }
                            if (existant != true)
                            {
                                listOtherPlayer.Remove(listOtherPlayer[i - 1]);
                            }
                        }

                    }



                    Console.WriteLine(responseData);
                    // _listJoueur.ad
                }
                #endregion
            }
        }
        public void idClient()
        {
            while (true)
            {



                byte[] data = new byte[300];
                String responseData = string.Empty;
                if (networkStream.DataAvailable == true)
                {
                    Int32 bytes = networkStream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine(responseData);
                    break;
                }
            }
        }
        public void envoieMessage(string donneJoueur)
        {
            string message = donneJoueur.ToString();
            // message = Console.ReadLine();
            NetworkStream stream;
            string text;
            text = message + '|';
            byte[] byteArrayASCII;
            // text += "#";
            byteArrayASCII = System.Text.Encoding.ASCII.GetBytes(text);
            stream = client.GetStream();
            //     stream.WriteTimeout = 0;
            Debug.WriteLine(byteArrayASCII.Length);
            stream.Write(byteArrayASCII, 0, byteArrayASCII.Length);

        }
        public Client()// TODO add bool is leader or not
        {
            //IPAddress ipAddress = Dns.GetHostEntry("Dns du serveur").AddressList[0];
            this.client = new TcpClient("10.5.42.41", 5732);

            //   clientLeader = new Client();
            networkStream = client.GetStream();
            string text = "Je suis leader:" + Game1.player.id + "|";
            byte[] byteArrayASCII;
            // text += "#";
            byteArrayASCII = System.Text.Encoding.ASCII.GetBytes(text);
            networkStream.Write(byteArrayASCII, 0, byteArrayASCII.Length);
            sock = client.Client;
            //clientLeader.envoieMessage("Je suis leader:" + Game1._player._id);

            Thread ctThread = new Thread(refreshMessage);
            ctThread.Start();


            // ReceiveMessage();
            // connectionServeur = true;
        }


    }
}
