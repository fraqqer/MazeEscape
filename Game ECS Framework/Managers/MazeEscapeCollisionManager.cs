using Ajax.Components;
using Ajax.Managers;
using OpenGL_Game.Scenes;
using OpenTK;

namespace OpenGL_Game.Managers
{
    class MazeEscapeCollisionManager : CollisionManager
    {
        private GameScene gameScene;
        public MazeEscapeCollisionManager(GameScene scene)
        {
            gameScene = scene;
        }
        public override void ProcessCollisions()
        {
            foreach (Collision collision in collisionManifold)
            {
                // implement sound when picking up
                if (collision.entity.Name == "Room One Key" || collision.entity.Name == "Room Two Key" || collision.entity.Name == "Room Three Key")
                {
                    KeyPickup(collision);
                }
                if (collision.entity.Name == "Portal Enabled")
                {
                    Portal(collision);
                }
                if (collision.entity.Name == "Drone" || collision.entity.Name == "Room One Ball")
                {
                    LoseLife(collision);
                }

                #region Wall Collision
                if (collision.entity.Name == "North Entrance Left Wall" || collision.entity.Name == "North Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.Z = -2.999999f;
                }
                if (collision.entity.Name == "South Entrance Right Wall" || collision.entity.Name == "South Entrance Left Wall")
                {
                    gameScene.camera.cameraPosition.Z = 2.999999f;
                }
                if (collision.entity.Name == "East Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.X = 2.999999f;
                }
                if (collision.entity.Name == "East Entrance Left Wall")
                {
                    gameScene.camera.cameraPosition.X = 2.499999f;
                }
                if (collision.entity.Name == "West Entrance Left Wall" || collision.entity.Name == "West Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.X = -2.499999f;
                }
                if (collision.entity.Name == "East Hallway Left Wall" || collision.entity.Name == "West Hallway Right Wall")
                {
                    gameScene.camera.cameraPosition.Z = -0.999999f;
                }
                if (collision.entity.Name == "East Hallway Right Wall" || collision.entity.Name == "West Hallway Left Wall")
                {
                    gameScene.camera.cameraPosition.Z = 0.9999999f;
                }
                if (collision.entity.Name == "North Hallway Left Wall" || collision.entity.Name == "South Hallway Left Wall")
                {
                    gameScene.camera.cameraPosition.X = -0.49999998f;
                }
                if (collision.entity.Name == "North Hallway Right Wall" || collision.entity.Name == "South Hallway Right Wall")
                {
                    gameScene.camera.cameraPosition.X = 0.499999f;
                }
                if (collision.entity.Name == "Room Four South Hallway Left Wall" || collision.entity.Name == "Room Three North Hallway Right Wall")
                {
                    gameScene.camera.cameraPosition.X = -7.5000001f;
                }
                if (collision.entity.Name == "Room Four South Entrance Left Wall" || collision.entity.Name == "Room Four South Entrance Right Wall" || collision.entity.Name == "Room One South Entrance Right Wall" || collision.entity.Name == "Room One South Entrance Left Wall")
                {
                    gameScene.camera.cameraPosition.Z = -5.50000111f;
                }
                if (collision.entity.Name == "Room Four West Entrance Left Wall" || collision.entity.Name == "Room Four West Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.X = -5.5000011f;
                }
                if (collision.entity.Name == "Room Four West Hallway Right Wall" || collision.entity.Name == "Room One West Hallway Left Wall")
                {
                    gameScene.camera.cameraPosition.Z = -7.500001f;
                }
                if (collision.entity.Name == "North Hallway Horizontal Wall")
                {
                    gameScene.camera.cameraPosition.Z = -8.999999f;
                }
                if (collision.entity.Name == "Room Four North Wall" || collision.entity.Name == "Room One North Wall")
                {
                    gameScene.camera.cameraPosition.Z = -10.99999f;
                }
                if (collision.entity.Name == "Room Four West Wall" || collision.entity.Name == "Room Three West Wall")
                {
                    gameScene.camera.cameraPosition.X = -10.499999f;
                }
                if (collision.entity.Name == "Room One East Wall" || collision.entity.Name == "Room Two East Wall")
                {
                    gameScene.camera.cameraPosition.X = 10.499999f;
                }
                if (collision.entity.Name == "Room One South Hallway Right Wall" || collision.entity.Name == "Room Two North Hallway Left Wall")
                {
                    gameScene.camera.cameraPosition.X = 7.500001f;
                }
                if (collision.entity.Name == "Room Two North Entrance Left Wall" || collision.entity.Name == "Room Two North Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.Z = 5.500001f;
                }
                if (collision.entity.Name == "Room One West Entrance Right Wall" || collision.entity.Name == "Room One West Entrance Left Wall" || collision.entity.Name == "Room Two West Entrance Right Wall" || collision.entity.Name == "Room Two West Entrance Left Wall")
                {
                    gameScene.camera.cameraPosition.X = 5.5500001f;
                }
                if (collision.entity.Name == "Room Two South Wall" || collision.entity.Name == "Room Three South Wall")
                {
                    gameScene.camera.cameraPosition.Z = 10.5000011f;
                }
                if (collision.entity.Name == "Room Two West Hallway Right Wall" || collision.entity.Name == "Room Three East Hallway Left Wall")
                {
                    gameScene.camera.cameraPosition.Z = 7.50001f;
                }
                if (collision.entity.Name == "Room Three East Entrance Left Wall" || collision.entity.Name == "Room Three East Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.X = -5.50001f;
                }
                if (collision.entity.Name == "Room Three North Entrance Left Wall" || collision.entity.Name == "Room Three North Entrance Right Wall")
                {
                    gameScene.camera.cameraPosition.Z = 5.50001f;
                }
                if (collision.entity.Name == "East Hallway Horizontal Wall")
                {
                    gameScene.camera.cameraPosition.X = 8.499999f;
                }
                if (collision.entity.Name == "South Hallway Horizontal Wall")
                {
                    gameScene.camera.cameraPosition.Z = 8.999999f;
                }
                if (collision.entity.Name == "West Hallway Horizontal Wall")
                {
                    gameScene.camera.cameraPosition.X = -8.49999f;
                }
                #endregion
            }
            collisionManifold.Clear();
        }

        private void KeyPickup(Collision coll)
        {
            IComponent positionComponent = coll.entity.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            ((ComponentPosition)positionComponent).Position -= new Vector3(0, 3, 0);

            GameScene.keysRemaining--;

            if (coll.entity.Name == "Room One Key")
            {
                IComponent audioComponent = coll.entity.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ((ComponentAudio)audioComponent).Play();

                GameScene.roomOneKeyCollected = true;
            }
            else if (coll.entity.Name == "Room Two Key")
            {
                IComponent audioComponent = coll.entity.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ((ComponentAudio)audioComponent).Play();

                GameScene.roomTwoKeyCollected = true;
            }
            else
            {
                IComponent audioComponent = coll.entity.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ((ComponentAudio)audioComponent).Play();

                GameScene.roomThreeKeyCollected = true;
            }
        }

        private void Portal(Collision coll)
        {
            if (GameScene.roomOneKeyCollected && GameScene.roomTwoKeyCollected && GameScene.roomThreeKeyCollected)
                gameScene.Win();
        }

        private void LoseLife(Collision coll)
        {
            if (coll.entity.Name == "Drone")
            {
                IComponent positionComponent = coll.entity.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                ((ComponentPosition)positionComponent).Position = new Vector3(0, 2.5f, 0);
            }

            gameScene.camera.cameraPosition = gameScene.startCameraPos;

            if (GameScene.livesRemaining == 1)
                gameScene.Lose();

            else
                GameScene.livesRemaining--;
        }
    }
}
