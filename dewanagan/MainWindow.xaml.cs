using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace dewanagan
{
   
    public partial class MainWindow :  Window 
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();
        int enemySpriteCounter = 0;
        int enemyCounnter = 100;
        int PlayerSpeed = 10;
        int Limite = 50;
        int score = 0;
        int damage = 0;
        int enemySpeed = 10;

        Rect playerHitBox; 

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();



            Mycanvas.Focus();


            ImageBrush bg = new ImageBrush();

            bg.ImageSource = new BitmapImage(new Uri("Pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0,0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            Mycanvas.Background = bg;


            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,/images/player.png"));
            Player.Fill = playerImage;  


        }
         

        private void GameLoop(object sender, EventArgs e)
        {

            playerHitBox = new Rect(Canvas.GetLeft(Player),Canvas.GetTop(Player), Player.Width, Player.Height);

            enemyCounnter -= 1;

            scoreText.Content = "score: " + score;
            damageText.Content = "Damage" + damage;

            if (enemyCounnter < 0)
            {
                MakeEnemies();
                enemyCounnter = Limite;

            }

            if(moveLeft == true && Canvas.GetLeft(Player) > 0)
            {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) - PlayerSpeed);
            }
             if ( moveRight == true && Canvas.GetLeft(Player) + 90 < Application.Current.MainWindow.Width)
             {
                Canvas.SetLeft(Player, Canvas.GetLeft(Player) + PlayerSpeed);
             }


             foreach( var x in Mycanvas.Children.OfType<Rectangle>())
             {

                if(x is Rectangle  && (string)x.Tag =="bullet" )
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                   
                    Rect bulletHitbOX = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                     if(Canvas.GetTop(x) < 10 )
                     { 

                        itemRemover.Add(x);
                     }

                     foreach ( var y  in Mycanvas.Children.OfType<Rectangle>())
                     {
                        
                        if(y is Rectangle && (string)y.Tag == "enemy" )
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitbOX.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score++;
                            }
                        }


                     }
                    
                    
                }

                if(x is Rectangle &&(string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    if( Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        damage += 10;

                    }
                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if(playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        itemRemover.Add(x);
                        damage += 5;
                    }
                }



             }
             foreach ( Rectangle i in itemRemover)
             {
                Mycanvas.Children.Remove(i);
             }
            


            if(score > 5)
            {
                Limite = 20;
                enemySpeed = 15;
            }
            if( damage > 99)
            {

                gameTimer.Stop();
                damageText.Content = "Damage:100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("you lost the game " + score + "Ailian ships" + Environment.NewLine + " Press Ok to play Again", "yusuf says: ");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            
        }

        private void OneKeyDown(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Left)
            {

                moveLeft = true;
            }
            if(e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OneKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {

                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Blue


                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(Player) + Player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(Player) - newBullet.Height);

                Mycanvas.Children.Add(newBullet);

            }
        
            
        }


        private void MakeEnemies()
        {
            
            ImageBrush enemySprite = new ImageBrush();

            enemySpriteCounter = rand.Next(1, 5);

            
            switch(enemySpriteCounter)
            {
            
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
                    break;
               
            }

            Rectangle newEnemy = new Rectangle
            {

                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite

            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30,430));
            Mycanvas.Children.Add(newEnemy);



        }

    }
}
