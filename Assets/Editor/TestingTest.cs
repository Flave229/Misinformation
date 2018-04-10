using NUnit.Framework;
using Moq;
using Assets.Scripts.General;
using Assets.Scripts.Conversation;

namespace Assets.Editor
{
    public class TestingTest
    {
        [Test]
        public void TestingTestSimplePasses()
        {
            Mock<IGeneral> general1 = new Mock<IGeneral>();
            general1.Setup(x => x.GetKnowledge()).Returns(10);
            general1.Setup(x => x.GetTrust()).Returns(10);
            
            Mock<IGeneral> general2 = new Mock<IGeneral>();
            general2.Setup(x => x.GetKnowledge()).Returns(10);
            general2.Setup(x => x.GetTrust()).Returns(10);

            ConversationManager conversationManager = ConversationManager.Instance();
            conversationManager.ConversationGenerator(general1.Object, general2.Object);
        }
    }
}