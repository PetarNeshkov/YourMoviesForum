using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using YourMoviesForum.Data.Models;

using static YourMoviesForum.Common.GlobalConstants.Administrator;

namespace YourMoviesForum.Data.Seeding
{
    public class PostsSeeder : ISeeder
    {
        public async Task SeedAsync(YourMoviesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Posts.AnyAsync())
            {
                return;
            }

            var adminId = await dbContext.Users
                              .Where(u => u.UserName == AdministratorUsername)
                              .Select(u => u.Id)
                              .FirstOrDefaultAsync();

            var createdOn= DateTime.UtcNow.ToLocalTime().ToString("dd/MM/yyyy H:mm");

            var posts = new List<Post>
            {
                new Post
                {
                    Title =@"Spider-Man: No Way Home Is 2021's Best Reviewed Movie",
                    Content=@"<p><strong><a href=""https://www.cbr.com/tag/spider-man-3/"" target = ""_blank"" rel = ""noopener""><em>Spider-Man: No Way Home</em></a>&nbsp;has nabbed&nbsp;another accolade in its run for box-office domination: the Golden Tomato Award from film review aggregator&nbsp;<a href=""https://www.cbr.com/tag/rotten-tomatoes/""  target = ""_blank"" rel = ""noopener"">Rotten Tomatoes</a>.</strong></p>
< p >< strong > In a tweet, Rotten Tomatoes noted, ""#SpiderManNoWayHome wins the #GoldenTomato Award for Best-Reviewed Movie of 2021,"" topping a list of 30 films released during the year, including those that went to theaters, streaming services or both.</ strong ></ p >
     < p > &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;< iframe src = ""//www.youtube.com/embed/JfVOs4VSpmA"" width = ""806"" height = ""452"" allowfullscreen = ""allowfullscreen"" ></ iframe ></ p >
               < p >< em > Spider - Man: No Way Home </ em > &nbsp; scored 93 % on the Tomatometer based on 374 reviews up to Dec. 31, 2021.& nbsp; Rotten Tomatoes collects film and TV reviews from multiple sources and notes whether they are positive or negative. The Tomatometer dubs each production & nbsp; ""fresh"" if 60 % &nbsp; or more of the reviews are positive; those with an average below that threshold are marked ""rotten."" Rotten Tomatoes notes that its ranking formula compensates for differences in how many reviews any given film gets.</ p >
                                    < p > Viewers were even more enthusiastic about & nbsp;< em > Spider - Man: No Way Home </ em >, leading to a & nbsp; 98 % fresh audience score.&nbsp; The ranking is based on 25,000 + ""verified ratings"" from users who&nbsp; were confirmed&nbsp; to have bought tickets for the film. Reflecting its popularity at the box office, &nbsp;< em > Spider - Man: No Way Home </ em > &nbsp; raced its way up the list of highest-grossing films with a worldwide take of $1.54 billion.The film currently sits at #8 on that list.</p>
<p> Only three superhero films made the best-reviewed list, and<em> & nbsp; No Way Home </ em > &nbsp;is the only & nbsp; one in the top 10.& nbsp;< em > The Suicide Squad</ em >, Warner Bros. Pictures' second take on the band of super-powered convicts, came in at #11, followed by Marvel Studios' & nbsp;< em > Shang - Chi and the Legend of the Ten Rings </ em > &nbsp; at & nbsp;#12.</p>
< p >< em > Spider - Man: No Way Home </ em > &nbsp;is, like the other Spider-Man films starring Tom Holland, a collaboration between Marvel Studios and Sony Pictures Entertainment. Sony & nbsp; holds the rights to films and spinoffs featuring the character and other supporting characters, heroes and villains from the Spider-Man comics.The Sony Spider - Man Universe includes 2018's&nbsp;<em>Venom</em>,&nbsp;2021's & nbsp;< em > Venom: Let There Be Carnage</ em > &nbsp; and the upcoming & nbsp;< a href = ""https://www.cbr.com/spider-man-morbius-delay-new-mutants-trending/""  target = ""_blank"" rel = ""noopener"" >< em > Morbius </ em >, which recently was delayed to April 1 </ a >.Projects in development include movies featuring Kraven the Hunter, the Sinister Six(a team - up of Spider - Man's fiercest foes), Madame Web and a third&nbsp;<em>Venom</em>&nbsp;movie. Holland is scheduled to appear as Spider-Man in an unspecified Marvel Cinematic Universe film, and Marvel and Sony are collaborating on a fourth film in Holland's series.</ p >
                                      < p >< em > Spider - Man: No Way Home </ em > &nbsp;is playing in theaters now. Digital copies will be sold at $19.99 starting Feb. 28, but fans can pre-order it now&nbsp; on Google Play, Vudu, Amazon Prime Video and Apple TV.</ p >",
                    AuthorId=adminId,
                    CategoryId=await dbContext.Categories
                               .Where(c => c.Name =="Action")
                               .Select(c => c.Id)
                               .FirstOrDefaultAsync(),
                    CreatedOn =createdOn,
                    Tags= await dbContext.Tags.Where(t=>t.Name=="Performance" || t.Name=="Success").ToListAsync()

                },
                new Post
                {
                    Title=@"Actor Alec Baldwin shoots filmer dead with prop gun on set",
                    Content=@"<p><em>Director Joel Souza was also wounded in the incident on set of Rust that killed cinematographer Halyna Hutchins, authorities say.</em></p>
<p><em><img style=""display: block; margin - left: auto; margin - right: auto"" src=""https://www.aljazeera.com/wp-content/uploads/2021/10/063_1345530823.jpg?resize=770%2C513"" alt="" width=""733"" height=""518""></em></p>
< p > United States movie star Alec Baldwin has fired a prop gun on a film set, killing cinematographer Halyna Hutchins and wounding director Joel Souza, according to authorities.</ p >
   < p > Police said the incident occurred on Thursday on the set of independent feature film Rust at the Bonanza Creek Ranch, a production location south of Santa Fe, the capital of New Mexico state.</ p >
      < p > According to authorities, Hutchins was transported by helicopter to the University of New Mexico Hospital, where she was pronounced dead, while Souza was taken by ambulance to Christus St Vincent Regional Medical centre to undergo treatment for his injuries.</ p >
         < p > It was not immediately known how serious Souza’s injuries were, although actress Frances Fisher tweeted early Friday that “Souza texted me that he’s out of hospital.”</ p >
         < p >< img src = ""https://www.aljazeera.com/wp-content/uploads/2021/10/063_1090671998.jpg?w=770&amp;resize=770%2C512"" alt = "" width = ""733"" height = ""518"" ></ p >
         < p > A spokesperson for Baldwin said there was an accident on the set involving the misfire of a prop gun with blanks.</ p >
            < p >“Mr Baldwin was interviewed by detectives,” Santa Fe sheriff spokesman Juan Rios told the AFP news agency.</ p >
            < p >“He provided statements and answered their questions.He came in voluntarily and he left the building after he finished his interviews.No charges have been filed and no arrests have been made.”</ p >
            < p > The Santa Fe New Mexican reported Baldwin was seen on Thursday outside the sheriff’s office in tears, but attempts to get a comment from him were unsuccessful.</ p >
               < p > Baldwin later issued a statement on Twitter saying: “There are no words to convey my shock and sadness regarding the tragic accident that took the life of Halyna Hutchins, a wife, mother and deeply admired colleague of ours.”</ p >
            < p >“I am in touch with her husband, offering my support to him and his family.My heart is broken for her husband, their son, and all who knew and loved Halyna,” Baldwin said.</ p >
             < p > Deadline cited a source in the Santa Fe Sheriff’s Department as saying that Baldwin was questioned by investigators and later released.</ p >
             < p > Baldwin is a co - producer in the Western movie set in 1880s Kansas and plays the eponymous Rust, an outlaw grandfather of a 13 - year - old boy convicted of accidental murder.</ p >
             < p > Most recently famous for his impersonations of former US President Donald Trump on NBC’s comedy sketch show Saturday Night Live, Baldwin has a long history in film and television, including roles in Glengarry Glen Ross and 30 Rock.</ p > ",
                    AuthorId=adminId,
                    CategoryId=await dbContext.Categories
                               .Where(c => c.Name =="Tragedy")
                               .Select(c => c.Id)
                               .FirstOrDefaultAsync(),
                    CreatedOn=createdOn,
                    Tags= await dbContext.Tags.Where(t=>t.Name=="Actors").ToListAsync()
                },
                new Post
                {
                    Title=@"Matrix 4 Resurrections:the first opinions on the sequel to the trilogy",
                    Content =@"<p><img style=""display: block; margin - left: auto; margin - right: auto"" src=""https://www.ecranlarge.com/media/cache/637x637/uploads/image/001/395/matrix-4-resurrections-photo-yahya-abdul-mateen-ii-1395194.png"" alt="" width=""740"" height=""310""></p>
< p dir = ""ltr"" lang = ""en"" >< strong > The first opinions on & nbsp;< em > Matrix Resurrections </ em >, fourth installment of the adventures of Neo, have arrived. The way to see if the quality of&nbsp;< em > Matrix </ em > &nbsp;is timeless.</ strong ></ p >
                        < p dir = ""ltr"" lang = ""en"" >< em >< strong > Spider - Man: No Way Home </ strong ></ em > &nbsp;is perhaps the most anticipated movie of 2021 by comic book and Marvel fans, but&nbsp;< em >< strong >< a href = ""https://www.ecranlarge.com/films/1095107-the-matrix-4"" target = ""_blank"" rel = ""noopener"" > Matrix Resurrections </ a ></ strong ></ em > &nbsp;is arguably the most desired and feared science fiction film of the year. Relaunching the Matrix after fifteen years of stoppage seems risky, especially since we had stopped on a conclusion that seemed quite final for the franchise.</ p >
                                                                < p dir = ""ltr"" lang = ""en"" > But Lana Wachowski & nbsp; the director tempted the devil with this fourth opus, whose promotion has continued & nbsp; to play deja-vu with its predecessors </ a >.A gimmick all the more dangerous as& nbsp;< em >< strong > Matrix </ strong > &nbsp;</ em > has always been recognized as a well of innovation, and not a saga looking too far into the navel. Obviously, we tend to believe that all this is a staging to better return our expectations, but what is the result of the maneuver? Review of the first tweeted opinions.</ p >
                                                                                                                   < p dir = ""ltr"" lang = ""en"" >< em >< strong >“Matrix Resurrections, despite (and thanks to) its endless awkwardness, is the most vivacious and daring Hollywood sequel since The Last Jedi.A silly / heartfelt reflection of reboot culture that makes peace with the self-centered creation process modern blockbusters. “</ strong > &nbsp; David Ehrlich – IndieWire </ em ></ p >
                                                                                                                                < p dir = ""ltr"" lang = ""en"" >< em >< strong >“Matrix Resurrections has been a lot of fun. The Evolution of the Movie fixes a lot of the issues I had with the 2 and 3.Lana Wachowski is so willful about everything, right down to the comments about Warner Bros.The Concepts are much more accessible and solid than in the first films too. “</ strong > &nbsp; Valerie Complex – Deadline </ em ></ p >
                                                                                                                                            < p dir = ""ltr"" lang = ""en"" >< em >< strong >“Matrix Resurrections is a big slap.Weird, romantic, extremely meta, and constantly funny. I think I liked it.Besides, everyone in this movie is ridiculously attractive.”</ strong > &nbsp; Chris Evangelista – Slashfilm </ em ></ p >
                                                                                                                                                        < p dir = ""ltr"" lang = ""en"" >< em >< img style = ""display: block; margin-left: auto; margin-right: auto"" src = ""https://www.ecranlarge.com/media/cache/resolve/637x637/uploads/image/001/409/matrix-resurrections-photo-jonathan-groff-1409105.jpg"" alt = "" width = ""466"" height = ""195"" ></ em ></ p >
                                                                                                                                                                         < p dir = ""ltr"" lang = ""en"" >< em >< strong >“The Matrix Resurrections is the most disappointing movie of the year.A convoluted mess that tarnishes the legacy of its franchise. There are solid performances in acting, but the story makes no sense and jokes tense me. It’s also too agreed and explicit for its own good. ”&nbsp;</strong>Scott Menzel – We Live Entertainment</em></p>
<p dir = ""ltr"" lang=""en""><img style = ""display: block; margin-left: auto; margin-right: auto"" title=""Anti-hindrance emergency evacuation"" src=""https://www.ecranlarge.com/media/cache/637x637/uploads/image/001/395/matrix-4-resurrections-photo-keanu-reeves-carrie-anne-moss-1395197.png"" alt=""Anti-hindrance emergency evacuation"" width=""466"" height=""195""></p>
<p style = ""text-align: center"" >< em > A </ em >< em > nti - hindrance emergency evacuation</em></p>
<p>Despite some dubious comments (tight humor, etc.), it seems that we have escaped the unsuccessful reboot,&nbsp;<strong>to have a sequel that will really serve the saga</strong>, while plotting its own path. On the other hand, there is still some doubt as to the quantity of surprises that awaits us, and to what extent the staging will know how to reinvent itself. Are we going to enjoy our session? Certainly. Are we holding a cinema revolution (s)? Not sure.Is the prospect of a new saga possible following this film? Mystery.</p>",
                    AuthorId=adminId,
                    CategoryId=await dbContext.Categories
                             .Where(c=>c.Name=="Action")
                             .Select(c=>c.Id)
                             .FirstOrDefaultAsync(),
                    CreatedOn=createdOn,
                    Tags= await dbContext.Tags.Where(t=>t.Name=="Cinema" || t.Name=="Performance").ToListAsync()
                }
            };

            await dbContext.Posts.AddRangeAsync(posts);
        }

    }
}
