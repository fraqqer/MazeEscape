using Ajax;
using Ajax.Components;
using Ajax.Objects;
using Ajax.Managers;
using Ajax.Scenes;
using Ajax.Systems;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        EntityManager entityManager;
        SystemManager systemManager;
        MazeEscapeCollisionManager gameCollisionManager;

        bool[] keysPressed = new bool[255];                             // 255 bytes
        public Vector3 startCameraPos = new Vector3(-9, 1, -9);         // 64 bytes
        private Vector3 droneMovement = new Vector3(1, 0, 1);           // 64 bytes
        public static Vector2 oldPosition;
        public static float dt = 0;                                     // 4 bytes
        private float cameraRotateSpeed = 0.03f;                        // 4 bytes
        private float cameraMoveSpeed = 0.1f;                           // 4 bytes
        public static uint keysRemaining = 3;                           // 4 bytes
        public static uint livesRemaining = 3;                          // 4 bytes
        public static bool roomOneKeyCollected, roomTwoKeyCollected, roomThreeKeyCollected = false; // 3 bytes
        public bool droneEnabled = true, collisionEnabled = true;       // 2 bytes

        public Camera camera;
        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            gameCollisionManager = new MazeEscapeCollisionManager(gameInstance);

            // Set the title of the window
            sceneManager.Title = "Maze Escape - Game Engine and Game Development Coursework";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(-9, 1, -9), new Vector3(0, 0, 100000), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            keysRemaining = 3;
            livesRemaining = 3;
            CreateEntities();
            CreateSystems();
            // TODO: Add your initialization logic here

        }
        private void CreateEntities()
        {
            #region Floors
            Entity lobbyFloor;

            lobbyFloor = new Entity("Lobby Floor");
            lobbyFloor.AddComponent(new ComponentPosition(0, 0, 0));
            lobbyFloor.AddComponent(new ComponentRotation(0, 0, 0));
            lobbyFloor.AddComponent(new ComponentScale(1, 1, 1));
            lobbyFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref lobbyFloor);

            Entity roomOneFloor;

            roomOneFloor = new Entity("Room One Floor");
            roomOneFloor.AddComponent(new ComponentPosition(8, 0, -8));
            roomOneFloor.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneFloor.AddComponent(new ComponentScale(1, 1, 1));
            roomOneFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref roomOneFloor);

            Entity roomTwoFloor;

            roomTwoFloor = new Entity("Room Two Floor");
            roomTwoFloor.AddComponent(new ComponentPosition(8, 0, 8));
            roomTwoFloor.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoFloor.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref roomTwoFloor);

            Entity roomThreeFloor;

            roomThreeFloor = new Entity("Room Three Floor");
            roomThreeFloor.AddComponent(new ComponentPosition(-8, 0, 8));
            roomThreeFloor.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeFloor.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref roomThreeFloor);

            Entity roomFourFloor;

            roomFourFloor = new Entity("Room Four Floor");
            roomFourFloor.AddComponent(new ComponentPosition(-8, 0, -8));
            roomFourFloor.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourFloor.AddComponent(new ComponentScale(1, 1, 1));
            roomFourFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref roomFourFloor);

            Entity northHallwayFloor;

            northHallwayFloor = new Entity("North Hallway Floor");
            northHallwayFloor.AddComponent(new ComponentPosition(0, 0, -6));
            northHallwayFloor.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayFloor.AddComponent(new ComponentScale(0.333f, 1, 1));
            northHallwayFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref northHallwayFloor);

            Entity northHallwayLeftHorizontalFloor;

            northHallwayLeftHorizontalFloor = new Entity("North Hallway Left Horizontal Floor");
            northHallwayLeftHorizontalFloor.AddComponent(new ComponentPosition(-4.5f, 0, -24));
            northHallwayLeftHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayLeftHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            northHallwayLeftHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref northHallwayLeftHorizontalFloor);

            Entity northHallwayRightHorizontalFloor;

            northHallwayRightHorizontalFloor = new Entity("North Hallway Right Horizontal Floor");
            northHallwayRightHorizontalFloor.AddComponent(new ComponentPosition(4.5f, 0, -24));
            northHallwayRightHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayRightHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            northHallwayRightHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref northHallwayRightHorizontalFloor);

            Entity southHallwayLeftHorizontalFloor;

            southHallwayLeftHorizontalFloor = new Entity("South Hallway Left Horizontal Floor");
            southHallwayLeftHorizontalFloor.AddComponent(new ComponentPosition(4.5f, 0, 24));
            southHallwayLeftHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayLeftHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            southHallwayLeftHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref southHallwayLeftHorizontalFloor);

            Entity southHallwayRightHorizontalFloor;

            southHallwayRightHorizontalFloor = new Entity("South Hallway Right Horizontal Floor");
            southHallwayRightHorizontalFloor.AddComponent(new ComponentPosition(-4.5f, 0, 24));
            southHallwayRightHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayRightHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            southHallwayRightHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref southHallwayRightHorizontalFloor);

            Entity eastHallwayHorizontalFloor;

            eastHallwayHorizontalFloor = new Entity("East Hallway Horizontal Floor");
            eastHallwayHorizontalFloor.AddComponent(new ComponentPosition(7.5f, 0, 0));
            eastHallwayHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            eastHallwayHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            eastHallwayHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref eastHallwayHorizontalFloor);

            Entity southHallwayHorizontalFloor;

            southHallwayHorizontalFloor = new Entity("South Hallway Floor");
            southHallwayHorizontalFloor.AddComponent(new ComponentPosition(0, 0, 6));
            southHallwayHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayHorizontalFloor.AddComponent(new ComponentScale(0.333f, 1, 1));
            southHallwayHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref southHallwayHorizontalFloor);

            Entity westHallwayHorizontalFloor;

            westHallwayHorizontalFloor = new Entity("West Hallway Horizontal Floor");
            westHallwayHorizontalFloor.AddComponent(new ComponentPosition(-7.5f, 0, 0));
            westHallwayHorizontalFloor.AddComponent(new ComponentRotation(0, 0, 0));
            westHallwayHorizontalFloor.AddComponent(new ComponentScale(0.6666f, 1, 0.3333f));
            westHallwayHorizontalFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref westHallwayHorizontalFloor);

            Entity eastVerticalHallwayFloor;

            eastVerticalHallwayFloor = new Entity("East Vertical Hallway Floor");
            eastVerticalHallwayFloor.AddComponent(new ComponentPosition(-24f, 0, 0));
            eastVerticalHallwayFloor.AddComponent(new ComponentRotation(0, 0, 0));
            eastVerticalHallwayFloor.AddComponent(new ComponentScale(0.333f, 1, 1.6666f));
            eastVerticalHallwayFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref eastVerticalHallwayFloor);

            Entity westVerticalHallwayFloor;

            westVerticalHallwayFloor = new Entity("West Vertical Hallway Floor");
            westVerticalHallwayFloor.AddComponent(new ComponentPosition(24f, 0, 0));
            westVerticalHallwayFloor.AddComponent(new ComponentRotation(0, 0, 0));
            westVerticalHallwayFloor.AddComponent(new ComponentScale(0.333f, 1, 1.6666f));
            westVerticalHallwayFloor.AddComponent(new ComponentGeometry("Geometry/Floor/Floor.obj"));
            entityManager.AddEntity(ref westVerticalHallwayFloor);

            #endregion

            #region North Entrance
            Entity northEntranceLeftWall, northEntranceRightWall;

            northEntranceLeftWall = new Entity("North Entrance Left Wall");
            northEntranceLeftWall.AddComponent(new Ajax.Components.ComponentPosition(-2, 1, -3));
            northEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            northEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            northEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            northEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref northEntranceLeftWall);

            northEntranceRightWall = new Entity("North Entrance Right Wall");
            northEntranceRightWall.AddComponent(new ComponentPosition(2, 1, -3));
            northEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            northEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            northEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            northEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref northEntranceRightWall);
            #endregion

            #region South Entrance
            Entity southEntranceLeftWall, southEntranceRightWall;

            southEntranceLeftWall = new Entity("South Entrance Left Wall");
            southEntranceLeftWall.AddComponent(new ComponentPosition(2, 1, 3));
            southEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            southEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            southEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            southEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref southEntranceLeftWall);

            southEntranceRightWall = new Entity("South Entrance Right Wall");
            southEntranceRightWall.AddComponent(new ComponentPosition(-2, 1, 3));
            southEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            southEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            southEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            southEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref southEntranceRightWall);
            #endregion

            #region East Entrance
            Entity eastEntranceRightWall, eastEntranceLeftWall;

            eastEntranceRightWall = new Entity("East Entrance Right Wall");
            eastEntranceRightWall.AddComponent(new ComponentPosition(3, 1, 2));
            eastEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            eastEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            eastEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            eastEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref eastEntranceRightWall);

            eastEntranceLeftWall = new Entity("East Entrance Left Wall");
            eastEntranceLeftWall.AddComponent(new ComponentPosition(3, 1, -2));
            eastEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            eastEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            eastEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            eastEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref eastEntranceLeftWall);
            #endregion

            #region West Entrance
            Entity westEntranceRightWall, westEntranceLeftWall;

            westEntranceRightWall = new Entity("West Entrance Right Wall");
            westEntranceRightWall.AddComponent(new ComponentPosition(-3, 1, -2));
            westEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            westEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            westEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            westEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref westEntranceRightWall);

            westEntranceLeftWall = new Entity("West Entrance Left Wall");
            westEntranceLeftWall.AddComponent(new ComponentPosition(-3, 1, 2));
            westEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            westEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            westEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            westEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref westEntranceLeftWall);
            #endregion

            #region East Hallway, East Entrance
            Entity eastHallwayLeftWall, eastHallwayRightWall;

            eastHallwayLeftWall = new Entity("East Hallway Left Wall");
            eastHallwayLeftWall.AddComponent(new ComponentPosition(5, 1, -1));
            eastHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            eastHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            eastHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall0.obj"));
            eastHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref eastHallwayLeftWall);

            eastHallwayRightWall = new Entity("East Hallway Right Wall");
            eastHallwayRightWall.AddComponent(new ComponentPosition(5, 1, 1));
            eastHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            eastHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            eastHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall180.obj"));
            eastHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref eastHallwayRightWall);
            #endregion

            #region West Hallway, West Entrance
            Entity westHallwayLeftWall, westHallwayRightWall;

            westHallwayLeftWall = new Entity("West Hallway Left Wall");
            westHallwayLeftWall.AddComponent(new ComponentPosition(-5, 1, 1));
            westHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            westHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            westHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall180.obj"));
            westHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref westHallwayLeftWall);

            westHallwayRightWall = new Entity("West Hallway Right Wall");
            westHallwayRightWall.AddComponent(new ComponentPosition(-5, 1, -1));
            westHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            westHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            westHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall0.obj"));
            westHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref westHallwayRightWall);
            #endregion

            #region North Hallway, North Entrance
            Entity northHallwayLeftWall, northHallwayRightWall;

            northHallwayLeftWall = new Entity("North Hallway Left Wall");
            northHallwayLeftWall.AddComponent(new ComponentPosition(-1, 1, -5));
            northHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            northHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall90.obj"));
            northHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref northHallwayLeftWall);

            northHallwayRightWall = new Entity("North Hallway Right Wall");
            northHallwayRightWall.AddComponent(new ComponentPosition(1, 1, -5));
            northHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            northHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall270.obj"));
            northHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref northHallwayRightWall);
            #endregion

            #region South Hallway, South Entrance
            Entity southHallwayLeftWall, southHallwayRightWall;

            southHallwayLeftWall = new Entity("South Hallway Left Wall");
            southHallwayLeftWall.AddComponent(new ComponentPosition(-1, 1, 5));
            southHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            southHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall90.obj"));
            southHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref southHallwayLeftWall);

            southHallwayRightWall = new Entity("South Hallway Right Wall");
            southHallwayRightWall.AddComponent(new ComponentPosition(1, 1, 5));
            southHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            southHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall270.obj"));
            southHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref southHallwayRightWall);
            #endregion

            #region Room Four/Three Hallway
            Entity roomFourSouthHallwayLeftWall, roomThreeNorthHallwayRightWall;

            roomFourSouthHallwayLeftWall = new Entity("Room Four South Hallway Left Wall");
            roomFourSouthHallwayLeftWall.AddComponent(new ComponentPosition(-7, 1, -3));
            roomFourSouthHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourSouthHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourSouthHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall270.obj"));
            roomFourSouthHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref roomFourSouthHallwayLeftWall);

            roomThreeNorthHallwayRightWall = new Entity("Room Three North Hallway Right Wall");
            roomThreeNorthHallwayRightWall.AddComponent(new ComponentPosition(-7, 1, 3));
            roomThreeNorthHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeNorthHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeNorthHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall270.obj"));
            roomThreeNorthHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref roomThreeNorthHallwayRightWall);
            #endregion

            #region Room 4 Entrance Walls
            Entity roomFourSouthEntranceLeftWall, roomFourSouthEntranceRightWall, roomFourEastEntranceLeftWall, roomFourEastEntranceRightWall;

            roomFourSouthEntranceLeftWall = new Entity("Room Four South Entrance Left Wall");
            roomFourSouthEntranceLeftWall.AddComponent(new ComponentPosition(-6, 1, -5));
            roomFourSouthEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourSouthEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourSouthEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            roomFourSouthEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomFourSouthEntranceLeftWall);

            roomFourSouthEntranceRightWall = new Entity("Room Four South Entrance Right Wall");
            roomFourSouthEntranceRightWall.AddComponent(new ComponentPosition(-10, 1, -5));
            roomFourSouthEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourSouthEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourSouthEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            roomFourSouthEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomFourSouthEntranceRightWall);

            roomFourEastEntranceLeftWall = new Entity("Room Four West Entrance Left Wall");
            roomFourEastEntranceLeftWall.AddComponent(new ComponentPosition(-5, 1, -10));
            roomFourEastEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourEastEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourEastEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            roomFourEastEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomFourEastEntranceLeftWall);

            roomFourEastEntranceRightWall = new Entity("Room Four West Entrance Right Wall");
            roomFourEastEntranceRightWall.AddComponent(new ComponentPosition(-5, 1, -6));
            roomFourEastEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourEastEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourEastEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            roomFourEastEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomFourEastEntranceRightWall);
            #endregion

            Entity roomFourEastHallwayRightWall, roomOneWestHallwayLeftWall;

            roomFourEastHallwayRightWall = new Entity("Room Four West Hallway Right Wall");
            roomFourEastHallwayRightWall.AddComponent(new ComponentPosition(-3, 1, -7));
            roomFourEastHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourEastHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourEastHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall180.obj"));
            roomFourEastHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref roomFourEastHallwayRightWall);

            roomOneWestHallwayLeftWall = new Entity("Room One West Hallway Left Wall");
            roomOneWestHallwayLeftWall.AddComponent(new ComponentPosition(3, 1, -7));
            roomOneWestHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneWestHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneWestHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall180.obj"));
            roomOneWestHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref roomOneWestHallwayLeftWall);

            Entity northHallwayHorizontalWall;

            northHallwayHorizontalWall = new Entity("North Hallway Horizontal Wall");
            northHallwayHorizontalWall.AddComponent(new ComponentPosition(0, 1, -9));
            northHallwayHorizontalWall.AddComponent(new ComponentRotation(0, 0, 0));
            northHallwayHorizontalWall.AddComponent(new ComponentScale(1, 1, 1));
            northHallwayHorizontalWall.AddComponent(new ComponentGeometry("Geometry/Wall/25mWall0.obj"));
            northHallwayHorizontalWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 5));
            entityManager.AddEntity(ref northHallwayHorizontalWall);

            Entity roomFourNorthWall, roomFourWestWall;

            roomFourNorthWall = new Entity("Room Four North Wall");
            roomFourNorthWall.AddComponent(new ComponentPosition(-8, 1, -11));
            roomFourNorthWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourNorthWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourNorthWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall0.obj"));
            roomFourNorthWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 3));
            entityManager.AddEntity(ref roomFourNorthWall);

            roomFourWestWall = new Entity("Room Four West Wall");
            roomFourWestWall.AddComponent(new ComponentPosition(-11, 1, -8));
            roomFourWestWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomFourWestWall.AddComponent(new ComponentScale(1, 1, 1));
            roomFourWestWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall90.obj"));
            roomFourWestWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 3));
            entityManager.AddEntity(ref roomFourWestWall);

            Entity roomOneNorthWall, roomOneEastWall;

            roomOneNorthWall = new Entity("Room One North Wall");
            roomOneNorthWall.AddComponent(new ComponentPosition(8, 1, -11));
            roomOneNorthWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneNorthWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneNorthWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall0.obj"));
            roomOneNorthWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 3));
            entityManager.AddEntity(ref roomOneNorthWall);

            roomOneEastWall = new Entity("Room One East Wall");
            roomOneEastWall.AddComponent(new ComponentPosition(11, 1, -8));
            roomOneEastWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneEastWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneEastWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall270.obj"));
            roomOneEastWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 3));
            entityManager.AddEntity(ref roomOneEastWall);

            Entity roomOneSouthEntranceLeftWall, roomOneSouthEntranceRightWall;

            roomOneSouthEntranceRightWall = new Entity("Room One South Entrance Right Wall");
            roomOneSouthEntranceRightWall.AddComponent(new ComponentPosition(6, 1, -5));
            roomOneSouthEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneSouthEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneSouthEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            roomOneSouthEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomOneSouthEntranceRightWall);

            roomOneSouthEntranceLeftWall = new Entity("Room One South Entrance Left Wall");
            roomOneSouthEntranceLeftWall.AddComponent(new ComponentPosition(10, 1, -5));
            roomOneSouthEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneSouthEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneSouthEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall180.obj"));
            roomOneSouthEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomOneSouthEntranceLeftWall);

            Entity roomOneWestEntranceLeftWall, roomOneWestEntranceRightWall;

            roomOneWestEntranceLeftWall = new Entity("Room One West Entrance Left Wall");
            roomOneWestEntranceLeftWall.AddComponent(new ComponentPosition(5, 1, -6));
            roomOneWestEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneWestEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneWestEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            roomOneWestEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomOneWestEntranceLeftWall);

            roomOneWestEntranceRightWall = new Entity("Room One West Entrance Right Wall");
            roomOneWestEntranceRightWall.AddComponent(new ComponentPosition(5, 1, -10));
            roomOneWestEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneWestEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneWestEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            roomOneWestEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomOneWestEntranceRightWall);

            Entity roomOneSouthHallwayRightWall, roomTwoNorthHallwayLeftWall;

            roomOneSouthHallwayRightWall = new Entity("Room One South Hallway Right Wall");
            roomOneSouthHallwayRightWall.AddComponent(new ComponentPosition(7, 1, -3));
            roomOneSouthHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneSouthHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneSouthHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall90.obj"));
            roomOneSouthHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref roomOneSouthHallwayRightWall);

            roomTwoNorthHallwayLeftWall = new Entity("Room Two North Hallway Left Wall");
            roomTwoNorthHallwayLeftWall.AddComponent(new ComponentPosition(7, 1, 3));
            roomTwoNorthHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoNorthHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoNorthHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall90.obj"));
            roomTwoNorthHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 2));
            entityManager.AddEntity(ref roomTwoNorthHallwayLeftWall);

            Entity roomTwoNorthEntranceLeftWall, roomTwoNorthEntranceRightWall;

            roomTwoNorthEntranceLeftWall = new Entity("Room Two North Entrance Left Wall");
            roomTwoNorthEntranceLeftWall.AddComponent(new ComponentPosition(6, 1, 5));
            roomTwoNorthEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoNorthEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoNorthEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            roomTwoNorthEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomTwoNorthEntranceLeftWall);

            roomTwoNorthEntranceRightWall = new Entity("Room Two North Entrance Right Wall");
            roomTwoNorthEntranceRightWall.AddComponent(new ComponentPosition(10, 1, 5));
            roomTwoNorthEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoNorthEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoNorthEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            roomTwoNorthEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomTwoNorthEntranceRightWall);

            #region Room Two

            Entity roomTwoEastWall, roomTwoSouthWall;

            roomTwoEastWall = new Entity("Room Two East Wall");
            roomTwoEastWall.AddComponent(new ComponentPosition(11, 1, 8));
            roomTwoEastWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoEastWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoEastWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall270.obj"));
            roomTwoEastWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 3));
            entityManager.AddEntity(ref roomTwoEastWall);

            roomTwoSouthWall = new Entity("Room Two South Wall");
            roomTwoSouthWall.AddComponent(new ComponentPosition(8, 1, 11));
            roomTwoSouthWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoSouthWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoSouthWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall180.obj"));
            roomTwoSouthWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 3));
            entityManager.AddEntity(ref roomTwoSouthWall);

            Entity roomTwoWestEntranceRightWall, roomTwoWestEntranceLeftWall;

            roomTwoWestEntranceRightWall = new Entity("Room Two West Entrance Right Wall");
            roomTwoWestEntranceRightWall.AddComponent(new ComponentPosition(5, 1, 6));
            roomTwoWestEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoWestEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoWestEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            roomTwoWestEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomTwoWestEntranceRightWall);

            roomTwoWestEntranceLeftWall = new Entity("Room Two West Entrance Left Wall");
            roomTwoWestEntranceLeftWall.AddComponent(new ComponentPosition(5, 1, 10));
            roomTwoWestEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoWestEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoWestEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall90.obj"));
            roomTwoWestEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomTwoWestEntranceLeftWall);

            Entity roomTwoWestHallwayRightWall, roomThreeEastHallwayLeftWall;

            roomTwoWestHallwayRightWall = new Entity("Room Two West Hallway Right Wall");
            roomTwoWestHallwayRightWall.AddComponent(new ComponentPosition(3, 1, 7));
            roomTwoWestHallwayRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoWestHallwayRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomTwoWestHallwayRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall0.obj"));
            roomTwoWestHallwayRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref roomTwoWestHallwayRightWall);

            #endregion

            roomThreeEastHallwayLeftWall = new Entity("Room Three East Hallway Left Wall");
            roomThreeEastHallwayLeftWall.AddComponent(new ComponentPosition(-3, 1, 7));
            roomThreeEastHallwayLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeEastHallwayLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeEastHallwayLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/10mWall0.obj"));
            roomThreeEastHallwayLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 2));
            entityManager.AddEntity(ref roomThreeEastHallwayLeftWall);

            Entity roomThreeEastEntranceLeftWall, roomThreeEastEntranceRightWall;

            roomThreeEastEntranceLeftWall = new Entity("Room Three East Entrance Left Wall");
            roomThreeEastEntranceLeftWall.AddComponent(new ComponentPosition(-5, 1, 6));
            roomThreeEastEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeEastEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeEastEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            roomThreeEastEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomThreeEastEntranceLeftWall);

            roomThreeEastEntranceRightWall = new Entity("Room Three East Entrance Right Wall");
            roomThreeEastEntranceRightWall.AddComponent(new ComponentPosition(-5, 1, 10));
            roomThreeEastEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeEastEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeEastEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall270.obj"));
            roomThreeEastEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 1));
            entityManager.AddEntity(ref roomThreeEastEntranceRightWall);

            Entity roomThreeNorthEntranceLeftWall, roomThreeNorthEntranceRightWall;

            roomThreeNorthEntranceLeftWall = new Entity("Room Three North Entrance Left Wall");
            roomThreeNorthEntranceLeftWall.AddComponent(new ComponentPosition(-10, 1, 5));
            roomThreeNorthEntranceLeftWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeNorthEntranceLeftWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeNorthEntranceLeftWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            roomThreeNorthEntranceLeftWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomThreeNorthEntranceLeftWall);

            roomThreeNorthEntranceRightWall = new Entity("Room Three North Entrance Right Wall");
            roomThreeNorthEntranceRightWall.AddComponent(new ComponentPosition(-6, 1, 5));
            roomThreeNorthEntranceRightWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeNorthEntranceRightWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeNorthEntranceRightWall.AddComponent(new ComponentGeometry("Geometry/Wall/5mWall0.obj"));
            roomThreeNorthEntranceRightWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 1));
            entityManager.AddEntity(ref roomThreeNorthEntranceRightWall);

            Entity roomThreeSouthWall, roomThreeWestWall;

            roomThreeSouthWall = new Entity("Room Three South Wall");
            roomThreeSouthWall.AddComponent(new ComponentPosition(-8, 1, 11));
            roomThreeSouthWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeSouthWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeSouthWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall180.obj"));
            roomThreeSouthWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 3));
            entityManager.AddEntity(ref roomThreeSouthWall);

            roomThreeWestWall = new Entity("Room Three West Wall");
            roomThreeWestWall.AddComponent(new ComponentPosition(-11, 1, 8));
            roomThreeWestWall.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeWestWall.AddComponent(new ComponentScale(1, 1, 1));
            roomThreeWestWall.AddComponent(new ComponentGeometry("Geometry/Wall/15mWall90.obj"));
            roomThreeWestWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 3));
            entityManager.AddEntity(ref roomThreeWestWall);

            #region Large 25m Walls (minus North Wall)
            Entity eastHallwayHorizontalWall, southHallwayHorizontalWall, westHallwayHorizontalWall;

            eastHallwayHorizontalWall = new Entity("East Hallway Horizontal Wall");
            eastHallwayHorizontalWall.AddComponent(new ComponentPosition(9, 1, 0));
            eastHallwayHorizontalWall.AddComponent(new ComponentRotation(0, 0, 0));
            eastHallwayHorizontalWall.AddComponent(new ComponentScale(1, 1, 1));
            eastHallwayHorizontalWall.AddComponent(new ComponentGeometry("Geometry/Wall/25mWall270.obj"));
            eastHallwayHorizontalWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 5));
            entityManager.AddEntity(ref eastHallwayHorizontalWall);

            southHallwayHorizontalWall = new Entity("South Hallway Horizontal Wall");
            southHallwayHorizontalWall.AddComponent(new ComponentPosition(0, 1, 9));
            southHallwayHorizontalWall.AddComponent(new ComponentRotation(0, 0, 0));
            southHallwayHorizontalWall.AddComponent(new ComponentScale(1, 1, 1));
            southHallwayHorizontalWall.AddComponent(new ComponentGeometry("Geometry/Wall/25mWall180.obj"));
            southHallwayHorizontalWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.HORIZONTAL, 5));
            entityManager.AddEntity(ref southHallwayHorizontalWall);

            westHallwayHorizontalWall = new Entity("West Hallway Horizontal Wall");
            westHallwayHorizontalWall.AddComponent(new ComponentPosition(-9, 1, 0));
            westHallwayHorizontalWall.AddComponent(new ComponentRotation(0, 0, 0));
            westHallwayHorizontalWall.AddComponent(new ComponentScale(1, 1, 1));
            westHallwayHorizontalWall.AddComponent(new ComponentGeometry("Geometry/Wall/25mWall90.obj"));
            westHallwayHorizontalWall.AddComponent(new ComponentCollisionLine(ComponentCollisionLine.DIRECTION.VERTICAL, 5));
            entityManager.AddEntity(ref westHallwayHorizontalWall);
            #endregion

            #region Keys
            Entity roomThreeKey;

            roomThreeKey = new Entity("Room Three Key");
            roomThreeKey.AddComponent(new ComponentPosition(-9f, 0.5f, 9f));
            roomThreeKey.AddComponent(new ComponentRotation(0, 0, 0));
            roomThreeKey.AddComponent(new ComponentScale(1f, 1f, 1f));
            roomThreeKey.AddComponent(new ComponentGeometry("Geometry/Key/Key.obj"));
            roomThreeKey.AddComponent(new ComponentCollisionSphere(0.001f));
            roomThreeKey.AddComponent(new ComponentAudio("audio/key.wav", false, false, 0, dt));
            entityManager.AddEntity(ref roomThreeKey);

            Entity roomTwoKey;

            roomTwoKey = new Entity("Room Two Key");
            roomTwoKey.AddComponent(new ComponentPosition(9f, 0.5f, 9f));
            roomTwoKey.AddComponent(new ComponentRotation(0, 0, 0));
            roomTwoKey.AddComponent(new ComponentScale(1f, 1f, 1f));
            roomTwoKey.AddComponent(new ComponentGeometry("Geometry/Key/Key.obj"));
            roomTwoKey.AddComponent(new ComponentCollisionSphere(0.001f));
            roomTwoKey.AddComponent(new ComponentAudio("audio/key.wav", false, false, 0, dt));
            entityManager.AddEntity(ref roomTwoKey);

            Entity roomOneKey;

            roomOneKey = new Entity("Room One Key");
            roomOneKey.AddComponent(new ComponentPosition(9f, 0.5f, -9f));
            roomOneKey.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneKey.AddComponent(new ComponentScale(1f, 1f, 1f));
            roomOneKey.AddComponent(new ComponentGeometry("Geometry/Key/Key.obj"));
            roomOneKey.AddComponent(new ComponentCollisionSphere(0.001f));
            roomOneKey.AddComponent(new ComponentAudio("audio/key.wav", false, false, 0, dt));
            entityManager.AddEntity(ref roomOneKey);
            #endregion

            Entity roomOneBall;

            roomOneBall = new Entity("Room One Ball");
            roomOneBall.AddComponent(new ComponentPosition(9, 1, -10));
            roomOneBall.AddComponent(new ComponentRotation(0, 0, 0));
            roomOneBall.AddComponent(new ComponentScale(1, 1, 1));
            roomOneBall.AddComponent(new ComponentGeometry("Geometry/Moon/Moon.obj"));
            roomOneBall.AddComponent(new ComponentCollisionSphere(0.001f));
            roomOneBall.AddComponent(new ComponentArtificialIntelligence(new Vector3(0, 0, 1), 0.0166667f, droneEnabled));
            entityManager.AddEntity(ref roomOneBall);

            Entity portalEnabled;

            portalEnabled = new Entity("Portal Enabled");
            portalEnabled.AddComponent(new ComponentPosition(-9, 0, -9));
            portalEnabled.AddComponent(new ComponentRotation(0, 0, 0));
            portalEnabled.AddComponent(new ComponentScale(1, 1, 1));
            portalEnabled.AddComponent(new ComponentGeometry("Geometry/Portal/portal.obj"));
            portalEnabled.AddComponent(new ComponentCollisionSphere(0.5f));
            entityManager.AddEntity(ref portalEnabled);

            Entity drone;

            drone = new Entity("Drone");
            drone.AddComponent(new ComponentPosition(0, 2.5f, 0));
            drone.AddComponent(new ComponentRotation(0, 0, 0));
            drone.AddComponent(new ComponentScale(1, 1, 1));
            drone.AddComponent(new ComponentGeometry("Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            drone.AddComponent(new ComponentCollisionSphere(0.05f));
            drone.AddComponent(new ComponentArtificialIntelligence(1, 0, 1, 0.0166667f, droneEnabled));
            drone.AddComponent(new ComponentAudio("audio/buzz.wav", true, true, 5, dt));
            entityManager.AddEntity(ref drone);
        }
        private void CreateSystems()
        {
            ISystem renderSystem, physicsSystem, audioSystem, collisionCameraSphereSystem, collisionCameraLineSystem, aiSystem;

            renderSystem = new SystemRender(ref camera);
            systemManager.AddSystem(ref renderSystem);
            physicsSystem = new SystemPhysics();
            systemManager.AddSystem(ref physicsSystem);
            audioSystem = new SystemAudio();
            systemManager.AddSystem(ref audioSystem);
            collisionCameraSphereSystem = new SystemCollisionCameraSphere(gameCollisionManager, camera);
            systemManager.AddSystem(ref collisionCameraSphereSystem);
            collisionCameraLineSystem = new SystemCollisionCameraLine(gameCollisionManager, camera);
            systemManager.AddSystem(ref collisionCameraLineSystem);
            aiSystem = new SystemArtificialIntelligence(ref camera);
            systemManager.AddSystem(ref aiSystem);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>

        public override void Update(FrameEventArgs e)
        {
            oldPosition = camera.cameraPosition.Xz;

            dt = (float)e.Time;
            // System.Console.WriteLine("fps=" + (int)(1.0/dt));
            Console.WriteLine($"{camera.cameraPosition.Xz}");

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);

            KeyboardPresses();

            if (collisionEnabled)
                gameCollisionManager.ProcessCollisions();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(ref entityManager);

            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            // Render lives
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), $"Lives: {livesRemaining}", 18, StringAlignment.Near, Color.Red);
            GUI.Label(new Rectangle(590, 0, (int)width, (int)(fontSize * 1f)), $"Keys Remaining: {keysRemaining}", 18, StringAlignment.Near, Color.White);

            if (roomOneKeyCollected)
                GUI.Label(new Rectangle(630, 40, (int)width, (int)(fontSize * 1f)), $"Room One Key", 18, StringAlignment.Near, Color.Yellow);

            if (roomTwoKeyCollected)
                GUI.Label(new Rectangle(630, 60, (int)width, (int)(fontSize * 1f)), $"Room Two Key", 18, StringAlignment.Near, Color.Yellow);

            if (roomThreeKeyCollected)
                GUI.Label(new Rectangle(610, 80, (int)width, (int)(fontSize * 1f)), $"Room Three Key", 18, StringAlignment.Near, Color.Yellow);

            GUI.Render();
        }
        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            roomOneKeyCollected = false;
            roomTwoKeyCollected = false;
            roomThreeKeyCollected = false;
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;
            ResourceManager.RemoveAllAssets();
            entityManager.CloseComponents();
        }
        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }
        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            if (keysPressed[(char)Key.Number2] || keysPressed[(char)Key.Keypad2])
            {
                if (collisionEnabled)
                    collisionEnabled = false;
                else
                    collisionEnabled = true;
            }

            if (keysPressed[(char)Key.Number1] || keysPressed[(char)Key.Keypad1])
            {
                if (droneEnabled) { droneEnabled = false; }
                else { droneEnabled = true; }
            }

            keysPressed[(char)e.Key] = false;
        }
        private void KeyboardPresses()
        {
            if (keysPressed[(char)Key.Up] || keysPressed[(char)Key.W])
            {
                camera.MoveForward(cameraMoveSpeed);
            }
            if (keysPressed[(char)Key.Down] || keysPressed[(char)Key.S])
            {
                camera.MoveForward(-cameraMoveSpeed);
            }
            if (keysPressed[(char)Key.Left] || keysPressed[(char)Key.A])
            {
                camera.RotateY(-cameraRotateSpeed);
            }
            if (keysPressed[(char)Key.Right] || keysPressed[(char)Key.D])
            {
                camera.RotateY(cameraRotateSpeed);
            }
        }

        public void Win()
        {
            if (roomOneKeyCollected && roomTwoKeyCollected && roomThreeKeyCollected)
                sceneManager.ChangeScene(SceneManager.SceneTypes.SCENE_WIN);
        }
        public void Lose()
        {
            sceneManager.ChangeScene(SceneManager.SceneTypes.SCENE_GAME_OVER);
        }
    }
}
