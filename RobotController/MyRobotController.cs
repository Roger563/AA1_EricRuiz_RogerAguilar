using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotController
{

    public struct MyQuat
    {
        public float w;
        public float x;
        public float y;
        public float z;

        public static MyQuat operator *(MyQuat a, MyQuat b)
        {
            float w = (a.w * b.w) - (a.x * b.x) - (a.y * b.y) - (a.z * b.z);
            float x = (a.w * b.x) + (a.x * b.w) + (a.y * b.z) - (a.z * b.y);
            float y = (a.w * b.y) - (a.x * b.z) + (a.y * b.w) + (a.z * b.x);
            float z = (a.w * b.z) + (a.x * b.y) - (a.y * b.x) + (a.z * b.w);

            MyQuat quatToReturn;
            quatToReturn.x = x;
            quatToReturn.y = y;
            quatToReturn.z = z;
            quatToReturn.w = w;

            return quatToReturn.Normalize();
        }

        public MyQuat Normalize()
        {
            float length = (float)Math.Sqrt((Math.Pow(this.x, 2.0f)) + (Math.Pow(this.y, 2.0f)) + (Math.Pow(this.z, 2.0f)) + (Math.Pow(this.w, 2.0f)));
            this.x /= length;
            this.y /= length;
            this.z /= length;
            this.w /= length;

            return this;
        }

        public MyQuat Inverse()
        {
            MyQuat invQuat;
            invQuat.x = -this.x;
            invQuat.y = -this.y;
            invQuat.z = -this.z;
            invQuat.w = this.w;
            return invQuat;
        }
    }

    public struct MyVec
    {

        public float x;
        public float y;
        public float z;
    }






    public class MyRobotController
    {

        private float r0_initRotation = 74.0f;
        private float r0_endRotation = 40.0f;

        private float r1_initRotation = -14.0f;
        private float r1_endRotation = -7.0f;

        private float r2_initRotation = 116.0f;
        private float r2_endRotation = 100.0f;

        bool e2_finished = false;
        // EXERCICIE 3
        private float r3Twist_initRotation = 0.0f;
        private float r3Swing_initRotation = 0.0f;
        private float e3_r0_endRotation = 40.0f;
        private float e3_r1_endRotation = -7.0f;
        private float e3_r2_endRotation = 100.0f;
        private float e3Twist_r3_endRotation = 90.0f;
        private float e3Swing_r3_endRotation = 0.0f;

        bool e3_finished = false;
        #region public methods

        public string Hi()
        {

            string s = "hello world from Eric and Roger";
            return s;

        }


        //EX1: this function will place the robot in the initial position

        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            e2_finished = false;
            lerpValue = 0;
            MyVec xAxis;
            xAxis.x = 1;
            xAxis.y = 0;
            xAxis.z = 0;

            MyVec yAxis;
            yAxis.x = 0;
            yAxis.y = 1;
            yAxis.z = 0;

            MyVec zAxis;
            zAxis.x = 0;
            zAxis.y = 0;
            zAxis.z = 1;

            //todo: change this, use the function Rotate declared below
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            rot0 = Rotate(rot0, yAxis, (float)(74.0f * (Math.PI / 180.0f)));
            rot1 = rot0 * Rotate(rot0, xAxis, (float)(-14.0f * (Math.PI / 180.0f)));
            rot2 = rot1 * Rotate(rot1, xAxis, (float)(116.0f * (Math.PI / 180.0f)));
            rot3 = rot2 * Rotate(rot2, yAxis, (float)(0.0f * (Math.PI / 180.0f)));
        }



        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            bool myCondition = false;
            //todo: add a check for your condition



            if (myCondition)
            {
                //todo: add your code here
                rot0 = NullQ;
                rot1 = NullQ;
                rot2 = NullQ;
                rot3 = NullQ;


                float newRot0 = r0_initRotation - ((r0_initRotation - r0_endRotation) * tParam);
                float newRot1 = r1_initRotation - ((r1_initRotation - r1_endRotation) * tParam);
                float newRot2 = r2_initRotation - ((r2_initRotation - r2_endRotation) * tParam);
               

                rot0 = Rotate(rot0, yAxis, (float)(newRot0 * (Math.PI / 180.0f)));
                rot1 = rot0 * Rotate(rot0, xAxis, (float)(newRot1 * (Math.PI / 180.0f)));
                rot2 = rot1 * Rotate(rot1, xAxis, (float)(newRot2 * (Math.PI / 180.0f)));
                rot3 = rot2 * Rotate(rot2, yAxis, (float)(0.0f * (Math.PI / 180.0f)));

                return true;
            }

            //todo: remove this once your code works.
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        static MyQuat totalRot = NullQ;
        
        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            MyVec xAxis;
            xAxis.x = 1;
            xAxis.y = 0;
            xAxis.z = 0;

            MyVec yAxis;
            yAxis.x = 0;
            yAxis.y = 1;
            yAxis.z = 0;

            MyVec zAxis;
            zAxis.x = 0;
            zAxis.y = 0;
            zAxis.z = 1;

            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            if (totalLerpValue > lerpValue && !e2_finished)
            {
                lerpValue += lerpSpeed;

                float tParam = lerpValue / totalLerpValue;

                float newRot0 = r0_initRotation - ((r0_initRotation - e3_r0_endRotation) * tParam);
                float newRot1 = r1_initRotation - ((r1_initRotation - e3_r1_endRotation) * tParam);
                float newRot2 = r2_initRotation - ((r2_initRotation - e3_r2_endRotation) * tParam);
                float newRot3Swing = r3Twist_initRotation - ((r3Twist_initRotation - e3Twist_r3_endRotation) * tParam);
                float newRot3Twist = r3Swing_initRotation - ((r3Swing_initRotation - e3Swing_r3_endRotation) * tParam);

                rot0 = Rotate(rot0, yAxis, (float)(newRot0 * (Math.PI / 180.0f)));
                rot1 = rot0 * Rotate(rot0, xAxis, (float)(newRot1 * (Math.PI / 180.0f)));
                rot2 = rot1 * Rotate(rot1, xAxis, (float)(newRot2 * (Math.PI / 180.0f)));
                rot3 = Rotate(rot2, xAxis, (float)(newRot3Swing * (Math.PI / 180.0f)))* Rotate(rot2, yAxis, (float)(newRot3Twist * (Math.PI / 180.0f)));//swing and twist rotation

                totalRot = rot2;
                return true;
            }
            else
            {
                lerpValue = 0.0f;
                e2_finished = true;
                return false;
            }
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            MyQuat quatToReturn = GetTwist(rot3) * totalRot.Inverse();
            quatToReturn.x *= -1;
            quatToReturn.y *= -1;
            quatToReturn.z *= -1;
            return totalRot* quatToReturn * rot3;
            

        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            MyQuat quatToReturn;
            quatToReturn.x = 0;
            quatToReturn.y = 0;
            quatToReturn.z = rot3.z * (float)(1 / Math.Sqrt(Math.Pow(rot3.w, 2) + Math.Pow(rot3.z, 2)));
            quatToReturn.w = rot3.w* (float)(1 / Math.Sqrt(Math.Pow(rot3.w, 2) + Math.Pow(rot3.z, 2)));
            return totalRot * quatToReturn;

        }




        #endregion


        #region private and internal methods

        internal int TimeSinceMidnight { get { return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond; } }


        private static MyQuat NullQ
        {
            get
            {
                MyQuat a;
                a.w = 1;
                a.x = 0;
                a.y = 0;
                a.z = 0;
                return a;

            }
        }

        internal MyQuat Multiply(MyQuat q1, MyQuat q2) {

            //todo: change this so it returns a multiplication:
            return NullQ;

        }

        internal MyQuat Rotate(MyQuat currentRotation, MyVec axis, float angle)
        {

            //todo: change this so it takes currentRotation, and calculate a new quaternion rotated by an angle "angle" radians along the normalized axis "axis"
            currentRotation.x = (float)(axis.x * Math.Sin(angle / 2));
            currentRotation.y = (float)(axis.y * Math.Sin(angle / 2));
            currentRotation.z = (float)(axis.z * Math.Sin(angle / 2));
            currentRotation.w = (float)(Math.Cos(angle / 2));
            return currentRotation;

        }




        //todo: add here all the functions needed

        #endregion






    }
}
