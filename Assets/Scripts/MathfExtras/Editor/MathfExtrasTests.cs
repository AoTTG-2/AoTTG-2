using NUnit.Framework;

public class MathfExtrasTests
{
    public class ClampAngleTests
    {
        [Test]
        public void Min_0_Max_180_Value_355_Returns_0()
        {
            const float min = 0f;
            const float max = 180f;
            const float value = 355f;
            const float expected = 0f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_0_Max_180_Value_185_Returns_180()
        {
            const float min = 0f;
            const float max = 180f;
            const float value = 185f;
            const float expected = 180f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_355_Max_5_Value_350_Returns_355()
        {
            const float min = 355f;
            const float max = 5f;
            const float value = 350f;
            const float expected = 355f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_355_Max_5_Value_10_Returns_5()
        {
            const float min = 355f;
            const float max = 5f;
            const float value = 10f;
            const float expected = 5f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_355_Max_5_Value_0_Returns_0()
        {
            const float min = 355f;
            const float max = 5f;
            const float value = 0f;
            const float expected = 0f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_0_Max_360_Value_0_Returns_0()
        {
            const float min = 0f;
            const float max = 360f;
            const float value = 0f;
            const float expected = 0f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Min_0_Max_360_Value_360_Returns_0()
        {
            const float min = 0f;
            const float max = 360f;
            const float value = 360f;
            const float expected = 0f;

            var result = MathfExtras.ClampAngle(value, min, max);
            
            Assert.AreEqual(expected, result);
        }
    }
}