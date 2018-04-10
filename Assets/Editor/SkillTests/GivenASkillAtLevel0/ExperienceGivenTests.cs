using Assets.Scripts.Progression;
using NUnit.Framework;

namespace Assets.Editor.SkillTests.GivenASkillAtLevel0
{
    class When999ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillStaysAtLevel0()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(999);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(0));
        }
    }

    class When1000ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel1()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(1000);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(1));
        }
    }

    class When1001ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel1()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(1001);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(1));
        }
    }

    class When2828ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel1()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(2828);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(1));
        }
    }

    class When2829ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel2()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(2829);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(2));
        }
    }

    class When5196ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel2()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(5196);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(2));
        }
    }

    class When5197ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel3()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(5197);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(3));
        }
    }

    class When7999ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel3()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(7999);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(3));
        }
    }

    class When8000ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel4()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(8000);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(4));
        }
    }

    class When8001ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel4()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(8001);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(4));
        }
    }

    class When11180ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel4()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(11180);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(4));
        }
    }

    class When11181ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel5()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(11181);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(5));
        }
    }

    class When14696ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel5()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(14696);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(5));
        }
    }

    class When14697ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel6()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(14697);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(6));
        }
    }

    class When18520ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel6()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(18520);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(6));
        }
    }

    class When18521ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel7()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(18521);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(7));
        }
    }

    class When22627ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel7()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(22627);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(7));
        }
    }

    class When22628ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel8()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(22628);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(8));
        }
    }

    class When26999ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel8()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(26999);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(8));
        }
    }

    class When27000ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel9()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(27000);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(9));
        }
    }

    class When27001ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel9()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(27001);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(9));
        }
    }

    class When31622ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel9()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(31622);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(9));
        }
    }

    class When31623ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel10()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(31623);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(10));
        }
    }

    class When99999ExperienceIsGiven
    {
        [Test]
        public void ThenTheSkillMovesToLevel10()
        {
            Skill testSkill = new Skill(0);
            testSkill.AddExperience(99999);

            Assert.That(testSkill.CurrentLevel, Is.EqualTo(10));
        }
    }
}