namespace CodeNet.Repository.Tests
{
    public class Assert2 : Assert
    {
        public static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = Newtonsoft.Json.JsonConvert.SerializeObject(expected);
            var actualJson = Newtonsoft.Json.JsonConvert.SerializeObject(actual);
            That(expectedJson, Is.EqualTo(actualJson));
        }

    }
}
