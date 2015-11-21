    using System;
    using System.Threading;
    using remoteApiNETWrapper;
    using R = CSharpSlam.Properties.Resources;

namespace CSharpSlam
{
    internal class Localization
    {
        private Pose _pose;

        // V-REP
        private float[] _pos = new float[3];
        private float[] _ori = new float[3];

        // kinematic model
        //  previous values
        private Pose _prevPose = new Pose(0, 0, 0);
        private float[] _prevWheelPositions= new float[2] {0, 0};
        private float _prevTime = 0;
        //  constants - must be corrected !!
        private const float l = 0.5f;
        private const float radius = 0.2f; //needed?

        public event EventHandler PoseChanged;

        public int ClientId { private get; set; }
        public int HandleSick { private get; set; }

        public double[,] CurrentRawDatas { private get; set; }
        public Layers Layers { private get; set; }

        public Pose Pose
        {
            get
            {
                return _pose;
            }

            private set
            {
                _pose = value;
                OnPoseChanged();
            }
        }

        public void CalculatePose()
        {
            VREPWrapper.simxGetObjectPosition(ClientId, HandleSick, -1, _pos, simx_opmode.oneshot_wait);
            VREPWrapper.simxGetObjectOrientation(ClientId, HandleSick, -1, _ori, simx_opmode.oneshot_wait);

            //mysterious formula
            if (_ori[0] < 0)
                _ori[1] = _ori[1] * -1 + (float)Math.PI / 2;
            else _ori[1] = _ori[1] + (float)Math.PI + (float)Math.PI / 2;

            Pose = new Pose((int)(_pos[0] * RobotControl.MapZoom), (int)(_pos[1] * RobotControl.MapZoom), 180.0 * _ori[0] / Math.PI);
        }
        
        public void CalculateSimpleKinematicPose() {
            // értékek megdása
            Pose pose = new Pose();
            float[] wheelPositions = new float[2];
            float time = 0;
            
            // sebesség kiszámítása
            //  szögsebesség kisz.

            // pose kiszámítása

            Pose = 1;
            
            // adatok mentése a következő számításhoz

        }

        private void OnPoseChanged()
        {
            if (PoseChanged != null)
            {
                PoseChanged(this, EventArgs.Empty);
            }
            ////TODO: kitorolni a c#5 kodot es cserelni 6-ra
            ////PoseChanged?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
