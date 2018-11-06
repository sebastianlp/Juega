using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Juega.Characters;
using Juega.Managers;
using System.Collections.Generic;
using System;
using Juega.Graphics;
using Juega.Meta;
using Microsoft.Xna.Framework.Input;



namespace Juega.Screens
{
    public class GamePlayScreen : GameScreen
    {
        Life life;
        Player player;
        List<Skeleton> enemies = new List<Skeleton>();
        Random random = new Random();
        float elapsedTimeSinceLastEnemyCreation = 0;

        public GamePlayScreen()
        {
            player = new Player();
            life = new Life();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            player.LoadContent();
            life.LoadContent();

            CreateEnemies();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            life.UnloadContent();

            foreach (Skeleton enemy in enemies)
            {
                enemy.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.P))
                ScreenManager.Instance.PauseScreen();

            elapsedTimeSinceLastEnemyCreation += (float)gameTime.ElapsedGameTime.TotalSeconds;
            List<int> indexesToRemove = indexesToRemove = new List<int>();

            if (enemies.Count > 0)
            {
                indexesToRemove = new List<int>();
                // Blucle por los enemigos de atras para adelante asi si ya salio de la pantalla o lo toco un disparo lo elimino
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    enemies[i].Update(gameTime);

                    // Si choca con el player y no ataco. Seteo el atributo que va a bajar la vida en true y el atributo que el enemigo ataco en true asi prevengo otro ataque del mismo enemigo
                    if (enemies[i].Image.Rectangle.Intersects(player.Image.Rectangle) && !enemies[i].Strike)
                    {
                        life.Hitted = true;
                        enemies[i].Strike = true;
                    }

                    if (player.Shoots.Count > 0)
                    {
                        List<int> shootIndexesToRemove = new List<int>();
                        for (int s = player.Shoots.Count - 1; s >= 0; s--)
                            if (enemies[i].Image.Rectangle.Intersects(player.Shoots[s].Image.Rectangle))
                            {
                                indexesToRemove.Add(i);
                                shootIndexesToRemove.Add(s); // Agrego el tiro para quitar
                            }

                        // Quito los tiros que interceptaron enemigos
                        foreach (int s in shootIndexesToRemove)
                        {
                            player.Shoots[s].UnloadContent();
                            player.Shoots.RemoveAt(s);
                        }
                    }

                    // Si llega abajo de todo y no lo agregue cuando dispare lo guardo. Ademas le saco el doble de vida al player.
                    if (enemies[i].Image.Position.Y > ScreenManager.Instance.ViewportHeight && !indexesToRemove.Contains(i))
                    {
                        indexesToRemove.Add(i);
                        life.Hitted = true;
                        life.Modifier = 2;
                    }
                }
            }

            // Quito los enemigos que llegaron al final o fueron interceptados por un tiro
            foreach (int i in indexesToRemove)
            {
                enemies[i].UnloadContent();
                enemies.RemoveAt(i);
            }

            if (enemies.Count < 15 && elapsedTimeSinceLastEnemyCreation > 1)// Si hay menos de 15 enemigos y hace mas de 1 segundo cree el anterior, voy de nuevo.
            {
                CreateEnemies();
                // Actualizo el ultimo que cree
                // Ojo, que si se envia la cantidad por parametro, se debe actualizar esa misma cantidad a partir del ultimo en la lista 
                // (esto es porque ya actualice todos los que tenia mas arriba, pero si inserto uno nuevo sin actualizarlo se ve TODO el sprite de skeletos y no queremos eso)
                enemies[enemies.Count - 1].Update(gameTime);
                elapsedTimeSinceLastEnemyCreation -= 1;
            }

            life.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            player.Draw(spriteBatch);
            life.Draw(spriteBatch);

            foreach (Skeleton enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        void CreateEnemies(int max = 1)
        {
            for(int x = 0; x < max; x++)
            {
                int randomX = random.Next(100, 480 - 100);
                int randomY = random.Next(0, 640 - 460);

                Skeleton enemy = new Skeleton();
                enemy.Image.Position.X = randomX;
                enemy.Image.Position.Y = randomY;

                enemy.LoadContent();

                enemies.Add(enemy);
            }
        }
    }
}
