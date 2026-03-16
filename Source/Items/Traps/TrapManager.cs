using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using M_Color = Microsoft.Xna.Framework.Color;

namespace Celeste.Mod.Celeste_Multiworld.Items.Traps
{
    public enum TrapType
    {
        Bald = 0x20,
        Literature = 0x21,
        Stun = 0x22,
        Invisible = 0x23,
        Fast = 0x24,
        Slow = 0x25,
        Ice = 0x26,
        Reverse = 0x28,
        Screen_Flip = 0x29,
        Laughter = 0x2A,
        Hiccup = 0x2B,
        Zoom = 0x2C
    }

    public enum TrapExpirationAction
    {
        Menu = 0,
        Deaths = 1,
        Screens = 2,
    }

    public class BaseTrapInstance
    {
        public string message;

        public TrapType type;
        public TrapExpirationAction expirationAction;
        public int expirationAmount;

        public DateTime activationTime;

        public bool bIsLinked = false;

        public int trackedDeaths = 0;
        public HashSet<string> trackedScreens = new HashSet<string>();

        public BaseTrapInstance(TrapType type, string message, TrapExpirationAction expirationAction, int expirationAmount, bool bIsLinked = false)
        {
            this.type = type;
            this.message = message;
            this.expirationAction = expirationAction;
            this.expirationAmount = expirationAmount;

            this.activationTime = DateTime.Now;
            this.bIsLinked = bIsLinked;
        }

        public void Activate()
        {
            switch (this.type)
            {
                case TrapType.Bald:
                {
                    break;
                }
                case TrapType.Literature:
                {
                    string chosenLit = TrapManager.Literature[Monocle.Calc.Random.Next(TrapManager.Literature.Count())];
                    ArchipelagoMessage message = new ArchipelagoMessage(chosenLit, ArchipelagoMessage.MessageType.Literature);
                    ArchipelagoManager.Instance.MessageLog.Insert(0, message);

                    this.activationTime = DateTime.Now;

                    break;
                }
                case TrapType.Stun:
                {
                    this.activationTime = DateTime.Now;

                    break;
                }
                case TrapType.Invisible:
                {
                    break;
                }
                case TrapType.Fast:
                {
                    break;
                }
                case TrapType.Slow:
                {
                    break;
                }
                case TrapType.Ice:
                {
                    break;
                }
                case TrapType.Reverse:
                {
                    break;
                }
                case TrapType.Screen_Flip:
                {
                    break;
                }
                case TrapType.Laughter:
                {
                    Level level = (Monocle.Engine.Scene as Level);
                    Player player = level.Entities.FindFirst<Player>();
                    if (player != null)
                    {
                        Hahaha laughter = new(player.Position, string.Empty, true, player.Position);
                        level.Add(laughter);
                        laughter.Enabled = true;
                    }

                    break;
                }
                case TrapType.Hiccup:
                {
                    break;
                }
                case TrapType.Zoom:
                {
                    break;
                }
            }
        }

        ~BaseTrapInstance()
        {
            switch (this.type)
            {
                case TrapType.Bald:
                {
                    break;
                }
                case TrapType.Literature:
                {
                    break;
                }
                case TrapType.Stun:
                {
                    break;
                }
                case TrapType.Invisible:
                {
                    break;
                }
                case TrapType.Fast:
                {
                    bool bOtherActive = TrapManager.Instance.IsTrapActive(TrapType.Slow);

                    if (!bOtherActive)
                    {
                        Monocle.Engine.TimeRate = 1.0f;
                    }
                    break;
                }
                case TrapType.Slow:
                {
                    bool bOtherActive = TrapManager.Instance.IsTrapActive(TrapType.Fast);

                    if (!bOtherActive)
                    {
                        Monocle.Engine.TimeRate = 1.0f;
                    }
                    break;
                }
                case TrapType.Ice:
                {
                    break;
                }
                case TrapType.Reverse:
                {
                    break;
                }
                case TrapType.Screen_Flip:
                {
                    break;
                }
                case TrapType.Laughter:
                {
                    Level level = (Monocle.Engine.Scene as Level);
                    if (level != null)
                    {
                        Hahaha laughter = level.Entities.FindFirst<Hahaha>();
                        if (laughter != null)
                        {
                            level.Remove(laughter);
                            laughter = null;
                        }

                        break;
                    }

                    break;
                }
                case TrapType.Hiccup:
                {
                    break;
                }
                case TrapType.Zoom:
                {
                    break;
                }
            }
        }
    }


    public class TrapManager : DrawableGameComponent
    {
        public static string[] Literature = [
            "{>> 0.25}I must not fear. Fear is the mind-killer. Fear is the little-death that brings about total obliteration. I will face my fear. I will permit it to pass over me and through me. And when it has gone past I will turn the inner eye to see its path. Where the fear has gone, there will be nothing. Only I will remain. {n}-Herbert",
            "{>> 0.25}About three things I was absolutely positive.{n}First, Edward was a vampire.{n}Second, there was a part of him - and I didn't know how potent that part might be - that thirsted for my blood.{n}And third, I was unconditionally and irrevocably in love with him. {n}-Meyer",
            "{>> 0.25}Say! I like green eggs and ham!{n}I do! I like them, Sam-I-am!{n}And I would eat them in a boat.{n}And I would eat them with a goat...{n}And I will eat them in the rain.{n}And in the dark.{n}And on a train.{n}And in a car. And in a tree.{n}They are so good, so good, you see!{n}So I will eat them in a box.{n}And I will eat them with a fox.{n}And I will eat them in a house.{n}And I will eat them with a mouse.{n}And I will eat them here and there.{n}Say! I will eat them anywhere!{n}I do so like green eggs and ham!{n}Thank you! Thank you, Sam-I-am! {n}-Seuss",
            "{>> 0.25}But, soft! what light through yonder window breaks? It is the east, and Juliet is the sun. Arise, fair sun, and kill the envious moon, Who is already sick and pale with grief, That thou her maid art far more fair than she: Be not her maid, since she is envious; Her vestal livery is but sick and green And none but fools do wear it; cast it off. It is my lady, O, it is my love! O, that she knew she were! {n}-Shakespeare",
            "{>> 0.25}It was the best of times, it was the worst of times, it was the age of wisdom, it was the age of foolishness, it was the epoch of belief, it was the epoch of incredulity, it was the season of light, it was the season of darkness, it was the spring of hope, it was the winter of despair. {n}-Dickens",
            "{>> 0.25}Marley was dead, to begin with. There is no doubt whatever, about that. The register of his burial was signed by the clergyman, the clerk, the undertaker, and the chief mourner. Scrooge signed it; and Scrooge's name was good upon 'change, for anything he chose to put his hand to. Old Marley was as dead as a door-nail. {n}-Dickens",
            "{>> 0.25}Many that live deserve death. And some that die deserve life. Can you give it to them? Then do not be too eager to deal out death in judgement. For even the very wise cannot see all ends. {n}-Tolkien",
            "{>> 0.25}It is a truth universally acknowledged, that a single man in possession of a good fortune, must be in want of a wife. {n}-Austen",
            "{>> 0.25}The truth always carries the ambiguity of the words used to express it. {n}-Herbert",
            "{>> 0.25}'I daresay you haven't had much practice,' said the Queen. 'When I was your age, I always did it for half-an-hour a day. Why, sometimes I've believed as many as six impossible things before breakfast.' {n}-Carroll",
            "{>> 0.25}Life, with its rules, its obligations, and its freedoms, is like a sonnet: You're given the form, but you have to write the sonnet yourself. {n}-L'Engle",
            "{>> 0.25}Like the moon over{n}the day my genius and brawn{n}are lost on these fools{n}-Haiku",
            "{>> 0.25}In a hole in the ground there lived a hobbit. Not a nasty, dirty, wet hole, filled with the ends of worms and an oozy smell, nor yet a dry, bare, sandy hole with nothing in it to sit down on or to eat: it was a hobbit-hole, and that means comfort. {n}-Tolkien",
            "{>> 0.25}'Good Morning!' said Bilbo, and he meant it. The sun was shining, and the grass was very green. But Gandalf looked at him from under long bushy eyebrows that stuck out further than the brim of his shady hat. 'What do you mean?' he said. 'Do you wish me a good morning, or mean that it is a good morning whether I want it or not, or that you feel good this morning, or that it is a morning to be good on?' 'All of them at once,' said Bilbo. {n}-Tolkien",
            "{>> 0.25}'Good morning!' he said at last. 'We don't want any adventures here, thank you! You might try over The Hill or across The Water.' By this he meant that the conversation was at an end. 'What a lot of things you do use Good morning for!' said Gandalf. 'Now you mean that you want to get rid of me, and that it won't be good till I move off.” {n}-Tolkien",
            "{>> 0.25}'There is nothing like looking, if you want to find something. You certainly usually find something, if you look, but it is not always quite the something you were after.' {n}-Tolkien",
            "{>> 0.25}Drummer, beat, and piper, blow.{n}Harper, strike, and soldier, go.{n}Free the flame and sear the grasses,{n}Til the dawning Red Star passes. {n}-McCaffrey",
            "{>> 0.25}The tears I feel today{n}I'll wait to shed tomorrow.{n}Though I'll not sleep this night{n}Nor find surcease from sorrow.{n}My eyes must keep their sight:{n}I dare not be tear-blinded.{n}I must be free to talk{n}Not choked with grief, clear-minded.{n}My mouth cannot betray{n}The anguish that I know.{n}Yes, I'll keep my tears til later:{n}But my grief will never go. {n}-McCaffrey",
            "{>> 0.25}Who wills,{n}Can.{n}Who tries,{n}Does.{n}Who loves,{n}Lives. {n}-McCaffrey",
            "{>> 0.25}The little queen all golden{n}Flew hissing at the sea.{n}To stop each wave{n}Her clutch to save{n}She ventured bravely.{n}As she attacked the sea in rage{n}A holderman came nigh{n}Along the sand{n}Fishnet in hand{n}And saw the queen midsky.{n}He stared at her in wonder{n}For often he'd been told{n}That such as she{n}Could never be{n}Who hovered there, bright gold.{n}He saw her plight and quickly{n}He looked up the cliff he faced{n}And saw a cave{n}Above the wave{n}In which her eggs he placed.{n}The little queen all golden{n}Upon his shoulder stood{n}Her eyes all blue{n}Glowed of her true{n}Undying gratitude. {n}-McCaffrey",
            "{>> 0.25}Harper, treat your words with care{n}For they may cause joy or despair{n}Sing your songs of health and love{n}Of dragons flaming from above. {n}-McCaffrey",
            "{>> 0.25}There was only one catch and that was Catch-22, which specified that a concern for one's safety in the face of dangers that were real and immediate was the process of a rational mind. Orr was crazy and could be grounded. All he had to do was ask, and as soon as he did, he would no longer be crazy and would have to fly more missions. Orr would be crazy to fly more missions and sane if he didn't, but if he was sane he had to fly them. If he flew them he was crazy and didn't have to, but if he didn't want to he was sane and had to. {n}-Heller",
            "{>> 0.25}'They're trying to kill me,' Yossarian told him calmly. 'No one's trying to kill you,' Clevinger cried. 'Then why are they shooting at me?' Yossarian asked. 'They're shooting at everyone,' Clevinger answered. 'They're trying to kill everyone.' And what difference does that make? {n}-Heller",
            "{>> 0.25}You have brains in your head. You have feet in your shoes.{n}You can steer yourself any direction you choose.{n}You're on your own. And you know what you know.{n}And YOU are the one who'll decide where to go... {n}-Suess",
            "{>> 0.25}When you have eliminated all which is impossible, then whatever remains, however improbable, must be the truth. {n}-Doyle",
            "{>> 0.25}'My mind,' he said, 'rebels at stagnation. Give me problems, give me work, give me the most abstruse cryptogram or the most intricate analysis, and I am in my own proper atmosphere. I can dispense then with artificial stimulants. But I abhor the dull routine of existence. I crave for mental exaltation. That is why I have chosen my own particular profession, or rather created it, for I am the only one in the world.' {n}-Doyle",
            "{>> 0.25}Life is infinitely stranger than anything which the mind of man could invent. We would not dare to conceive the things which are really mere commonplaces of existence. If we could fly out of that window hand in hand, hover over this great city, gently remove the roofs, and and peep in at the queer things which are going on, the strange coincidences, the plannings, the cross-purposes, the wonderful chains of events, working through generations, and leading to the most outre results, it would make all fiction with its conventionalities and foreseen conclusions most stale and unprofitable. {n}-Doyle",
            "{>> 0.25}The story so far:{n}In the beginning the Universe was created. This has made a lot of people very angry and been widely regarded as a bad move. {n}-Adams",
            "{>> 0.25}For instance, on the planet Earth, man had always assumed that he was more intelligent than dolphins because he had achieved so much-the wheel, New York, wars and so on-whilst all the dolphins had ever done was muck about in the water having a good time. But conversely, the dolphins had always believed that they were far more intelligent than man—for precisely the same reasons. {n}-Adams",
            "{>> 0.25}It is known that there are an infinite number of worlds, simply because there is an infinite amount of space for them to be in. However, not every one of them is inhabited. Therefore, there must be a finite number of inhabited worlds. Any finite number divided by infinity is as near to nothing as makes no odds, so the average population of all the planets in the Universe can be said to be zero. From this it follows that the population of the whole Universe is also zero, and that any people you may meet from time to time are merely the products of a deranged imagination. {n}-Adams",
            "{>> 0.25}Far over the misty mountains cold{n}To dungeons deep and caverns old{n}We must away,{n}ere break of day,{n}To seek the pale enchanted gold.{n}The dwarves of yore made mighty spells,{n}While hammers fell like ringing bells{n}In places deep,{n}where dark things sleep,{n}In hollow halls beneath the fells.{n}For ancient king and elvish lord{n}There many a gleaming golden hoard{n}They shaped and wrought,{n}and light they caught{n}To hide in gems on hilt of sword.{n}On silver necklaces they strung{n}The flowering stars, on crowns they hung{n}The dragon-fire,{n}in twisted wire{n}They meshed the light of moon and sun. {n}-Tolkien",
            "{>> 0.25}Far over the misty mountains cold{n}To dungeons deep and caverns old{n}We must away,{n}ere break of day,{n}To claim our long-forgotten gold.{n}Goblets they carved there for themselves And harps of gold, where no man delves{n}There lay they long,{n}and many a song{n}Was sung unheard by men or elves.{n}The pines were roaring on the height,{n}The winds were moaning in the night.{n}The fire was red,{n}it flaming spread.{n}The trees like torches blazed with light. {n}-Tolkien",
            "{>> 0.25}The bells were ringing in the dale{n}And men looked up with faces pale.{n}Then dragon's ire{n}more fierce than fire{n}Laid low their towers and houses frail.{n}The mountain smoked beneath the moon.{n}The dwarves, they heard the tramp of doom.{n}They fled their hall,{n}to dying fall{n}Beneath his feet, beneath the moon.{n}Far over the misty mountains grim{n}To dungeons deep and caverns dim{n}We must away,{n}ere break of day,{n}To win our harps and gold from him! {n}-Tolkien",
            "{>> 0.25}Roads go ever ever on,{n}Over rock and under tree,{n}By caves where never sun has shone,{n}By streams that never find the sea.{n}Over snow by winter sown,{n}And through the merry flowers of June,{n}Over grass and over stone,{n}And under mountains of the moon.{n}Roads go ever ever on{n}Under cloud and under star,{n}Yet feet that wandering have gone{n}Turn at last to home afar.{n}Eyes that fire and sword have seen{n}And horror in the halls of stone{n}Look at last on meadows green{n}And trees and hills they long have known. {n}-Tolkien",
            "{>> 0.25}Three Rings for the Elven-kings under the sky,{n}Seven for the Dwarf-lords in their halls of stone,{n}Nine for Mortal Men doomed to die,{n}One for the Dark Lord on his dark throne{n}In the Land of Mordor where the Shadows lie.{n}One Ring to rule them all, One Ring to find them,{n}One Ring to bring them all, and in the darkness bind them,{n}In the Land of Mordor where the Shadows lie. {n}-Tolkien",
            "{>> 0.25}All that is gold does not glitter,{n}Not all those who wander are lost.{n}The old that is strong does not wither,{n}Deep roots are not reached by the frost.{n}From the ashes a fire shall be woken,{n}A light from the shadows shall spring.{n}Renewed shall be blade that was broken,{n}The crownless again shall be king. {n}-Tolkien",
            "{>> 0.25}Gone away, gone ahead,{n}Echoes roll unanswered.{n}Empty, open, dusty, dead,{n}Why have all the Weyrfolk fled?{n}Where have dragons gone together?{n}Leaving Weyrs to wind and weather?{n}Setting herdbeasts free of tether?{n}Gone, our safeguards, gone, but whither?{n}Have they flown to some new Weyr{n}When cruel Threads some others fear?{n}Are they worlds away from here?{n}Why, oh, why, the empty Weyr? {n}-McCaffrey",
            "{>> 0.25}Love, which quickly arrests the gentle heart,{n}Seized him with my beautiful form{n}That was taken from me, in a manner which still grieves me.{n}Love, which pardons no beloved from loving,{n}took me so strongly with delight in him{n}That, as you see, it still abandons me not... {n}-Alighieri",
            "{>> 0.25}I cannot express it, but surely you and everybody have a notion that there is or should be an existence of yours beyond you. What were the use of my creation, if I were entirely contained here? My great miseries in this world have been Heathcliff's miseries, and I watched and felt each from the beginning: my great thought in living is himself. If all else perished, and he remained, I should still continue to be, and if all else remained, and he were annihilated, the universe would turn to a mighty stranger: I should not seem a part of it. {n}-Bronte",
            "{>> 0.25}I have dreamt in my life, dreams that have stayed with me ever after, and changed my ideas. They have gone through and through me, like wine through water, and altered the color of my mind. And this is one: I'm going to tell it - but take care not to smile at any part of it. {n}-Bronte",
            "{>> 0.25}This story shall the good man teach his son,{n}And Crispin Crispian shall ne'er go by,{n}From this day to the ending of the world,{n}But we in it shall be remembered.{n}We few, we happy few, we band of brothers.{n}For he to-day that sheds his blood with me{n}Shall be my brother. {n}-Shakespeare",
            "{>> 0.25}However mean your life is, meet it and live it. Do not shun it and call it hard names. It is not so bad as you are. It looks poorest when you are richest. The fault-finder will find faults even in paradise. Love your life, poor as it is. You may perhaps have some pleasant, thrilling, glorious hours, even in a poorhouse. The setting sun is reflected from the windows of the almshouse as brightly as from the rich man's abode; the snow melts before its door as early in the spring. I do not see but a quiet mind may live as contentedly there, and have as cheering thoughts, as in a palace. {n}-Thoreau",
            "{>> 0.25}We must learn to reawaken and keep ourselves awake, not by mechanical aids, but by an infinite expectation of the dawn, which does not forsake us even in our soundest sleep. I know of no more encouraging fact than the unquestionable ability of man to elevate his life by a conscious endeavour. It is something to be able to paint a particular picture, or to carve a statue, and so to make a few objects beautiful, but it is far more glorious to carve and paint the very atmosphere and medium through which we look, which morally we can do. To affect the quality of the day, that is the highest of arts. {n}-Thoreau",
            "{>> 0.25}If one advances confidently in the direction of his dreams, and endeavors to live the life which he has imagined, he will meet with a success unexpected in common hours. He will put some things behind, will pass an invisible boundary. New, universal, and more liberal laws will begin to establish themselves around and within him, or the old laws be expanded, and interpreted in his favor in a more liberal sense, and he will live with the license of a higher order of beings. {n}-Thoreau",
            "{>> 0.25}I'm nobody!{n}Who are you?{n}Are you nobody, too?{n}Then there's a pair of us, don't tell!{n}They'd banish us, you know.{n}How dreary to be somebody!{n}How public, like a frog{n}To tell your name the livelong day{n}To an admiring bog! -Dickinson",
            "{>> 0.25}How happy is the little stone{n}That rambles in the road alone,{n}And doesn't care about careers,{n}And exigencies never fears.{n}Whose coat of elemental brown{n}A passing universe put on.{n}And independent as the sun,{n}Associates or glows alone,{n}Fulfilling absolute decree{n}In casual simplicity. {n}-Dickinson",
            "{>> 0.25}Because I could not stop for death{n}He kindly stopped for me.{n}The carriage held but just ourselves{n}And immortality {n}-Dickinson",
            "{>> 0.25}For, like almost everyone else in our country, I started out with my share of optimism. I believed in hard work and progress and action, but now, after first being 'for' society and then 'against' it, I assign myself no rank or any limit, and such an attitude is very much against the trend of the times. But my world has become one of infinite possibilities. What a phrase - still it's a good phrase and a good view of life, and a man shouldn't accept any other; that much I've learned underground. Until some gang succeeds in putting the world in a strait jacket, its definition is possibility. {n}-Ellison",
            "{>> 0.25}Tarnished, a word.{n}{n}What if you booted up Resident Evil 4 Remake and Ashley was just a tiny mouse. What would you do?{n}{n}Just, imagine this: There she is standing there, cute as a button, and with a high-pitched voice she cries out: \"Leon! Help!I can't reach the Gorgonzola! Leon!\".{n}{n}And then after you have helped her up in some sort of quick-time mousecapade, she turns to you, looks you dead in the eyes and says: \"Cheese Whiz, mister! Thanks for the help!\". What would you do?{n}{n}{n}Anyways, find the Albinauric woman.",
            "{>> 0.25}I'm fully aware of what I'm doing. Can't you see? Man committed a sin... disturbing the life cycle of nature... The original sin that man is responsible to... To protect the life cycle. I have made a creature to rule over mankind... This is the final battle. Show yourself! Our new ruler, the Emperor!",
            "{>> 0.25}Light thinks it travels faster than anything but it is wrong. No matter how fast light travels, it finds the darkness has always got there first, and is waiting for it. {n}-Pratchet",
            "{>> 0.25}It would be difficult for me to tell you what the moral of this story is. For some stories, it's easy. The moral of 'The Three Bears,' for instance, is 'Never break into someone else's house.' The moral of 'Snow White' is 'Never eat apples.' The moral of World War I is 'Never assassinate Archduke Ferdinand. {n}-Snicket",
            "{>> 0.25}Tell me. For whom do you fight?{n}{n}Hmph! How very glib. And do you believe in Eorzea? Eorzea's unity is forged of falsehoods. Its city-states are built on deceit. And its faith is an instrument of deception.{n}{n}It is naught but a cobweb of lies. To believe in Eorzea is to believe in nothing.In Eorzea, the beast tribes often summon gods to fight in their stead--though your comrades only rarely respond in kind. Which is strange, is it not?{n}{n}Are the 'Twelve' otherwise engaged? I was given to understand they were your protectors. If you truly believe them your guardians, why do you not repeat the trick that served you so well at Carteneau, and call them down? They will answer--so long as you lavish them with crystals and gorge them on aether. Your gods are no different than those of the beasts--eikons every one. Accept but this, and you will see how Eorzea's faith is bleeding the land dry.",
        ];

        public static Dictionary<string, TrapType> TrapLinkNames = new Dictionary<string, TrapType>()
        {
            { "Bald Trap",          TrapType.Bald },
            { "Hiccup Trap",        TrapType.Hiccup },
            { "Ice Trap",           TrapType.Ice },
            { "Invisible Trap",     TrapType.Invisible },
            { "Laughter Trap",      TrapType.Laughter },
            { "Literature Trap",    TrapType.Literature },
            { "Fast Trap",          TrapType.Fast },
            { "Reverse Trap",       TrapType.Reverse },
            { "Screen Flip Trap",   TrapType.Screen_Flip },
            { "Slow Trap",          TrapType.Slow },
            { "Stun Trap",          TrapType.Stun },
            { "Zoom Trap",          TrapType.Zoom },

            { "Animal Bonus Trap",   TrapType.Literature },
            { "Banana Trap",         TrapType.Ice },
            { "Banana Peel Trap",    TrapType.Ice },
            { "Bonk Trap",           TrapType.Hiccup },
            { "Bullet Time Trap",    TrapType.Slow },
            { "Chaos Control Trap",  TrapType.Stun },
            { "Confuse Trap",        TrapType.Reverse },
            { "Cutscene Trap",       TrapType.Literature },
            { "Deisometric Trap",    TrapType.Zoom },
            { "Depletion Trap",      TrapType.Bald },
            { "Disable Tag Trap",    TrapType.Bald },
            { "Dry Trap",            TrapType.Bald },
            { "Eject Ability",       TrapType.Hiccup },
            { "Empty Item Box Trap", TrapType.Bald },
            { "Exposition Trap",     TrapType.Literature },
            { "Fear Trap",           TrapType.Hiccup },
            { "Fire Trap",           TrapType.Fast },
            { "Fish Eye Trap",       TrapType.Zoom },
            { "Flip Trap",           TrapType.Screen_Flip },
            { "Freeze Trap",         TrapType.Stun },
            { "Frog Trap",           TrapType.Hiccup },
            { "Frozen Trap",         TrapType.Stun },
            { "Fuzzy Trap",          TrapType.Laughter },
            { "Ghost",               TrapType.Invisible },
            { "Ghost Chat",          TrapType.Hiccup },
            { "Ice Floor Trap",      TrapType.Ice },
            { "Invisiball Trap",     TrapType.Invisible },
            { "Invisibility Trap",   TrapType.Invisible },
            { "Jump Trap",           TrapType.Hiccup },
            { "Jumping Jacks Trap",  TrapType.Hiccup },
            { "Mirror Trap",         TrapType.Screen_Flip },
            { "Paralysis Trap",      TrapType.Stun },
            { "Paralyze Trap",       TrapType.Stun },
            { "Phone Trap",          TrapType.Literature },
            { "Pixelate Trap",       TrapType.Zoom },
            { "Pixellation Trap",    TrapType.Zoom },
            { "Poison Trap",         TrapType.Hiccup },
            { "Possession Trap",     TrapType.Invisible },
            { "Powerpoint Trap",     TrapType.Slow },
            { "Reversal Trap",       TrapType.Reverse },
            { "Sleep Trap",          TrapType.Stun },
            { "Slip Trap",           TrapType.Ice },
            { "Slowness Trap",       TrapType.Slow },
            { "Spam Trap",           TrapType.Literature },
            { "Spotlight Trap",      TrapType.Zoom },
            { "Spring Trap",         TrapType.Hiccup },
            { "Swap Trap",           TrapType.Reverse },
            { "Text Trap",           TrapType.Literature },
            { "Time Limit Trap",     TrapType.Fast },
            { "Time Warp Trap",      TrapType.Fast },
            { "Timer Trap",          TrapType.Fast },
            { "Tutorial Trap",       TrapType.Literature },
        };
        public static Dictionary<int, int> EnabledTraps = new Dictionary<int, int>();

        public static TrapManager Instance { get; private set; }

        public static TrapExpirationAction ExpirationAction { get; set; } = TrapExpirationAction.Deaths;
        public static int ExpirationAmount { get; set; } = 5;
        public static int TRAP_COOLDOWN = 120;

        public int TrapCooldownTimer = TRAP_COOLDOWN;
        public List<BaseTrapInstance> ActiveTraps = new List<BaseTrapInstance>();
        public Queue<BaseTrapInstance> QueuedTraps = new Queue<BaseTrapInstance>();
        BaseTrapInstance PriorityTrap = null;

        public TrapManager(Game game) : base(game)
        {
            game.Components.Add(this);
            Instance = this;
        }
        public override void Update(GameTime gameTime)
        {
            if (!ArchipelagoManager.Instance.Ready)
            {
                return;
            }

            this.ActiveTraps.RemoveAll(trap => this.IsTrapExpired(trap));

            Level level = (Monocle.Engine.Scene as Level);

            if (level == null)
            {
                return;
            }

            if (this.TrapCooldownTimer > 0)
            {
                this.TrapCooldownTimer--;
                this.TickActiveTraps();
                return;
            }

            // Check Priority Trap for validity, discard if invalid
            if (this.PriorityTrap != null && this.IsTrapValid(this.PriorityTrap.type))
            {
                this.ActiveTraps.Add(this.PriorityTrap);
                this.PriorityTrap.Activate();

                Logger.Info("AP", this.PriorityTrap.message);
                ArchipelagoMessage message = new ArchipelagoMessage(this.PriorityTrap.message, ArchipelagoMessage.MessageType.ItemReceive, Archipelago.MultiClient.Net.Enums.ItemFlags.Trap);
                ArchipelagoManager.Instance.MessageLog.Insert(0, message);
                Monocle.Engine.Commands.Log(this.PriorityTrap.message, M_Color.DeepPink);
                Audio.Play(SFX.game_gen_bird_squawk);
            }

            this.PriorityTrap = null;

            // Dequeue a trap, check for validity, requeue if invalid
            if (this.QueuedTraps.Count > 0)
            {
                BaseTrapInstance newTrap = this.QueuedTraps.Dequeue();
                if (this.IsTrapValid(newTrap.type))
                {
                    this.ActiveTraps.Add(newTrap);
                    newTrap.Activate();

                    ArchipelagoManager.Instance.SendTrapLink(newTrap.type);

                    Logger.Info("AP", newTrap.message);
                    ArchipelagoMessage message = new ArchipelagoMessage(newTrap.message, ArchipelagoMessage.MessageType.ItemReceive, Archipelago.MultiClient.Net.Enums.ItemFlags.Trap);
                    ArchipelagoManager.Instance.MessageLog.Insert(0, message);
                    Monocle.Engine.Commands.Log(newTrap.message, M_Color.DeepPink);
                    Audio.Play(SFX.game_gen_bird_squawk);

                    this.TrapCooldownTimer = TRAP_COOLDOWN;
                }
                else
                {
                    this.QueuedTraps.Enqueue(newTrap);
                }
            }

            this.TickActiveTraps();
        }

        public void TickActiveTraps()
        {
            foreach (TrapType trapType in Enum.GetValues(typeof(TrapType)))
            {
                bool bActive = this.IsTrapActive(trapType);
                switch (trapType)
                {
                    case TrapType.Bald:
                    {
                        // Handled in modPlayer.cs
                        break;
                    }
                    case TrapType.Literature:
                    {
                        BaseTrapInstance trap = this.ActiveTraps.Find(trap => trap.type == TrapType.Literature);
                        if (trap != null && (DateTime.Now - trap.activationTime).TotalSeconds >= 6.0f)
                        {
                            this.ActiveTraps.Remove(trap);
                        }

                        break;
                    }
                    case TrapType.Stun:
                    {
                        // Mostly handled in modPlayer.cs
                        BaseTrapInstance trap = this.ActiveTraps.Find(trap => trap.type == TrapType.Stun);
                        if (trap != null && (DateTime.Now - trap.activationTime).TotalSeconds >= 3.0f)
                        {
                            this.ActiveTraps.Remove(trap);
                        }

                        break;
                    }
                    case TrapType.Invisible:
                    {
                        SaveData.Instance.Assists.InvisibleMotion = bActive;

                        break;
                    }
                    case TrapType.Fast:
                    {
                        if (SaveData.Instance.CurrentSession_Safe.Area.ID == 0)
                        {
                            // Prologue depends on Timescale
                            break;
                        }

                        if (bActive)
                        {
                            Monocle.Engine.TimeRate = 2.0f;
                        }

                        break;
                    }
                    case TrapType.Slow:
                    {
                        if (SaveData.Instance.CurrentSession_Safe.Area.ID == 0)
                        {
                            // Prologue depends on Timescale
                            break;
                        }

                        if (bActive)
                        {
                            Monocle.Engine.TimeRate = 0.5f;
                        }

                        break;
                    }
                    case TrapType.Ice:
                    {
                        SaveData.Instance.Assists.LowFriction = bActive;

                        break;
                    }
                    case TrapType.Reverse:
                    {
                        Input.MoveX.Inverted = bActive;
                        Input.MoveY.Inverted = bActive;
                        Input.Aim.InvertedX = bActive;
                        Input.Aim.InvertedY = bActive;

                        break;
                    }
                    case TrapType.Screen_Flip:
                    {
                        SaveData.Instance.Assists.MirrorMode = bActive;

                        break;
                    }
                    case TrapType.Laughter:
                    {
                        if (bActive)
                        {
                            Level level = (Monocle.Engine.Scene as Level);
                            Player player = level.Entities.FindFirst<Player>();
                            if (player != null)
                            {
                                Hahaha laughter = level.Entities.FindFirst<Hahaha>();
                                if (laughter == null)
                                {
                                    laughter = new(player.Position, string.Empty, true, player.Position);
                                    level.Add(laughter);
                                    laughter.Enabled = true;
                                }
                                else
                                {
                                    laughter.Position = player.Position;
                                }
                            }
                        }
                        break;
                    }
                    case TrapType.Hiccup:
                    {
                        SaveData.Instance.Assists.Hiccups = bActive;

                        break;
                    }
                    case TrapType.Zoom:
                    {
                        Level level = (Monocle.Engine.Scene as Level);
                        Player player = level.Entities.FindFirst<Player>();
                        if (player != null)
                        {
                            if (bActive)
                            {
                                level.Camera.Zoom = 2f;
                                level.Camera.Approach(player.Position, 0.1f);
                            }
                            else
                            {
                                level.Camera.Zoom = 1f;
                            }
                        }

                        break;
                    }
                }
            }
        }

        public void AddTrapToQueue(TrapType type, string message)
        {
            this.QueuedTraps.Enqueue(new BaseTrapInstance(type, message, ExpirationAction, ExpirationAmount));
        }

        public void SetPriorityTrap(TrapType type, string message)
        {
            this.PriorityTrap = new BaseTrapInstance(type, message, ExpirationAction, ExpirationAmount, true);
        }

        public bool IsTrapValid(TrapType type)
        {
            if (this.IsTrapActive(type))
            {
                return false;
            }

            Level level = (Monocle.Engine.Scene as Level);

            if (level == null)
            {
                return false;
            }

            if (SaveData.Instance.CurrentSession_Safe.Level == "void")
            {
                return false;
            }

            Player player = level.Entities.FindFirst<Player>();

            if (player == null)
            {
                return false;
            }

            switch (type)
            {
                case TrapType.Bald:
                {
                    if (player.Sprite == null || player.Sprite.HairCount == 0)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Literature:
                {
                    MiniTextbox textbox = level.Entities.FindFirst<MiniTextbox>();
                    if (textbox != null)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Stun:
                {
                    if (level.InCredits)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Invisible:
                {
                    break;
                }
                case TrapType.Fast:
                {
                    if (SaveData.Instance.CurrentSession_Safe.Area.ID == 0)
                    {
                        // Prologue depends on Timescale
                        return false;
                    }

                    bool bOtherActive = this.IsTrapActive(TrapType.Slow);

                    if (bOtherActive)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Slow:
                {
                    bool bOtherActive = this.IsTrapActive(TrapType.Fast);

                    if (bOtherActive)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Ice:
                {
                    if (level.InCredits)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Reverse:
                {
                    if (level.InCredits)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Screen_Flip:
                {
                    break;
                }
                case TrapType.Laughter:
                {
                    if (level.Entities.FindFirst<Hahaha>() != null)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Hiccup:
                {
                    if (level.InCredits)
                    {
                        return false;
                    }

                    break;
                }
                case TrapType.Zoom:
                {
                    if (level.InCredits)
                    {
                        return false;
                    }

                    break;
                }
            }

            return true;
        }

        public bool IsTrapActive(TrapType type)
        {
            return this.ActiveTraps.Where(item => item.type == type).Count() > 0;
        }

        public bool IsTrapExpired(BaseTrapInstance trap)
        {
            switch (trap.expirationAction)
            {
                case TrapExpirationAction.Screens:
                {
                    return trap.trackedScreens.Count > trap.expirationAmount;
                }
                case TrapExpirationAction.Deaths:
                {
                    return trap.trackedDeaths >= trap.expirationAmount;
                }
            }

            Level level = (Monocle.Engine.Scene as Level);

            if (level == null)
            {
                return true;
            }

            return false;
        }

        public void AddDeathToActiveTraps()
        {
            foreach (BaseTrapInstance trap in this.ActiveTraps)
            {
                trap.trackedDeaths += 1;
            }
        }

        public void AddScreenToActiveTraps(string screen)
        {
            foreach (BaseTrapInstance trap in this.ActiveTraps)
            {
                trap.trackedScreens.Add(screen);
            }
        }

        public void Reset()
        {
            this.ActiveTraps.Clear();
            this.QueuedTraps.Clear();
            this.PriorityTrap = null;
        }
    }
}
