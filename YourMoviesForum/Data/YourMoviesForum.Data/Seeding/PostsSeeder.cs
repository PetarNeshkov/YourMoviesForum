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
                    Content=@"<p><img style=""display: block; margin-left:auto; margin-right:auto"" src=""https://th.bing.com/th/id/OIP.1UOcBiFy28nzarnMIsd2tgHaD5?pid=ImgDet&amp;rs=1"" alt="" width=""700"" height=""430""></p>
<p>If you thought this was going to be the biggest Spider - Man movie ever, you might be right.With at least five villains, rumors of returning Spider-Men a <span><a href=""https://www.cnet.com/culture/entertainment/spider-man-no-way-home-trailer-tops-avengers-endgame-with-record-355-5-million-views/"" > record - breaking trailer </a></span> and the concept of the multiverse opening it all up<span><a href = ""https://www.cnet.com/culture/entertainment/spider-man-no-way-home-disney-plus-release-date-starz/""> Spider - Man: No Way Home </ a></span> &nbsp; plays just about every trump card it has to claim the title of next&nbsp;<span><a href = ""https://www.cnet.com/culture/entertainment/avengers-endgame-spoiler-filled-marvel-mcu-review-close-to-perfect/""> Avengers: Endgame </a></span>.</p>
                                             <p> For the most part, everything works. Just make sure you've seen all the previous Spider-Man movies. Despite an inevitably tangled web of characters, backstories and motivations, No Way Home manages a surprisingly tidy plot if you understand where each player comes from. Watch Spider-Man's back-catalog and you'll be in the know as to why the people at the back of the theater are cheering at any given moment.</p>
                                                <p> And if you're not intimately acquainted with two decades of previous Spider-Man films? If you're here for simply a well - oiled and entertaining;<a href=""https://www.cnet.com/marvel/""> Marvel </a> and <a href=""https://www.cnet.com/tags/sony/""> Sony </a>) flick, you won't be disappointed. You might not fully appreciate the scale of what's essentially the live-action equivalent of Spider - Man: Into the Spider - Verse, aka the<span><a href = ""https://www.cnet.com/culture/why-into-the-spider-verse-is-still-the-best-superhero-movie-ever-made/""> best(or second - best) Spider - Man movie ever</a></span>.But you'll be treated to likeable heroes with relatable character development; slick, dynamic actions scenes; weird, wacky humor; high stakes; powerful emotional punches; and at least one incredible one-liner, delivered by Aunt May (Marisa Tomei) of all people.</p>
                                                                                            <p> Basically, this is the Tom Holland Spider-Man movie that feels most like the previous Tobey Maguire and Andrew Garfield installments.Director Jon Watts deals with real consequences, a darker tone in general and a recognizable New York setting(with a few <a href=""https://www.cnet.com/how-to/marvel-cinematic-universe-timeline-avengers/""> Marvel Cinematic Universe</a> touches). In other words, the third Holland entry truly makes up for the filler that was Spider - Man: Far From Home.</p>",
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
<p><em><img style=""display:block; margin-left:auto;margin-right:auto"" src=""https://www.aljazeera.com/wp-content/uploads/2021/10/063_1345530823.jpg?resize=770%2C513"" alt="" width=""733"" height=""518""></em></p>
<p>United States movie star Alec Baldwin has fired a prop gun on a film set, killing cinematographer Halyna Hutchins and wounding director Joel Souza, according to authorities.</p>
   <p>Police said the incident occurred on Thursday on the set of independent feature film Rust at the Bonanza Creek Ranch, a production location south of Santa Fe, the capital of New Mexico state.</p>
      <p>According to authorities, Hutchins was transported by helicopter to the University of New Mexico Hospital, where she was pronounced dead, while Souza was taken by ambulance to Christus St Vincent Regional Medical centre to undergo treatment for his injuries.</p>
         <p>It was not immediately known how serious Souza’s injuries were, although actress Frances Fisher tweeted early Friday that “Souza texted me that he’s out of hospital.”</p>
         <p><img style=""display:block; margin-left:auto;margin-right:auto"" src=""https://www.aljazeera.com/wp-content/uploads/2021/10/063_1090671998.jpg?w=770&amp;resize=770%2C512"" alt="" width=""733"" height=""518""></p>
         <p> A spokesperson for Baldwin said there was an accident on the set involving the misfire of a prop gun with blanks.</p>
            <p>“Mr Baldwin was interviewed by detectives,” Santa Fe sheriff spokesman Juan Rios told the AFP news agency.</p>
            <p>“He provided statements and answered their questions.He came in voluntarily and he left the building after he finished his interviews.No charges have been filed and no arrests have been made.”</p>
            <p>The Santa Fe New Mexican reported Baldwin was seen on Thursday outside the sheriff’s office in tears, but attempts to get a comment from him were unsuccessful.</p>
               <p> Baldwin later issued a statement on Twitter saying: “There are no words to convey my shock and sadness regarding the tragic accident that took the life of Halyna Hutchins, a wife, mother and deeply admired colleague of ours.”</p>
            <p>“I am in touch with her husband, offering my support to him and his family.My heart is broken for her husband, their son, and all who knew and loved Halyna,” Baldwin said.</p>
             <p>Deadline cited a source in the Santa Fe Sheriff’s Department as saying that Baldwin was questioned by investigators and later released.</p>
             <p>Baldwin is a co-producer in the Western movie set in 1880s Kansas and plays the eponymous Rust, an outlaw grandfather of a 13-year-old boy convicted of accidental murder.</p>
             <p>Most recently famous for his impersonations of former US President Donald Trump on NBC’s comedy sketch show Saturday Night Live, Baldwin has a long history in film and television, including roles in Glengarry Glen Ross and 30 Rock.</p>",
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
                    Content =@"<p><img style=""display:block;margin-left:auto;margin-right:auto"" src=""https://www.ecranlarge.com/media/cache/637x637/uploads/image/001/395/matrix-4-resurrections-photo-yahya-abdul-mateen-ii-1395194.png"" alt="" width=""740"" height=""310""></p>
<p dir=""ltr"" lang=""en""><strong>The first opinions on <em>Matrix Resurrections</em>, fourth installment of the adventures of Neo, have arrived. The way to see if the quality of<em>Matrix</em>is timeless.</strong></p>
                        <p dir=""ltr"" lang=""en""><em><strong>Spider - Man: No Way Home </strong></em> is perhaps the most anticipated movie of 2021 by comic book and Marvel fans, but<em><strong><a href=""https://www.ecranlarge.com/films/1095107-the-matrix-4"" target =""_blank"" rel=""noopener""> Matrix Resurrections </a></strong></em> is arguably the most desired and feared science fiction film of the year. Relaunching the Matrix after fifteen years of stoppage seems risky, especially since we had stopped on a conclusion that seemed quite final for the franchise.</p>
                                                                <p dir=""ltr"" lang=""en""> But Lana Wachowski the director tempted the devil with this fourth opus, whose promotion has continued &nbsp; to play deja-vu with its predecessors </a>.A gimmick all the more dangerous as<em><strong> Matrix </strong> </em> has always been recognized as a well of innovation, and not a saga looking too far into the navel. Obviously, we tend to believe that all this is a staging to better return our expectations, but what is the result of the maneuver? Review of the first tweeted opinions.</p>
                                                                                                                   <p dir=""ltr"" lang=""en""><em><strong>“Matrix Resurrections, despite (and thanks to) its endless awkwardness, is the most vivacious and daring Hollywood sequel since The Last Jedi.A silly / heartfelt reflection of reboot culture that makes peace with the self-centered creation process modern blockbusters. “</strong>David Ehrlich–IndieWire</em></p>
                                                                                                                                <p dir=""ltr"" lang=""en""><em><strong>“Matrix Resurrections has been a lot of fun. The Evolution of the Movie fixes a lot of the issues I had with the 2 and 3.Lana Wachowski is so willful about everything, right down to the comments about Warner Bros.The Concepts are much more accessible and solid than in the first films too. “</strong> Valerie Complex – Deadline </em></p>
                                                                                                                                            <p dir=""ltr"" lang=""en""><em><strong>“Matrix Resurrections is a big slap.Weird, romantic, extremely meta, and constantly funny. I think I liked it.Besides, everyone in this movie is ridiculously attractive.”</strong> Chris Evangelista–Slashfilm</em></p>
                                                                                                                                                        <p dir=""ltr"" lang=""en""><em><img style=""display:block; margin-left:auto; margin-right:auto"" src=""https://www.ecranlarge.com/media/cache/resolve/637x637/uploads/image/001/409/matrix-resurrections-photo-jonathan-groff-1409105.jpg"" alt ="" width=""466"" height=""195""></em></p>
                                                                                                                                                                         <p dir=""ltr"" lang=""en""><em><strong>“The Matrix Resurrections is the most disappointing movie of the year.A convoluted mess that tarnishes the legacy of its franchise. There are solid performances in acting, but the story makes no sense and jokes tense me. It’s also too agreed and explicit for its own good.</strong>Scott Menzel – We Live Entertainment</em></p>
<p dir=""ltr"" lang=""en""><img style=""display:block; margin-left:auto; margin-right:auto"" title=""Anti-hindrance emergency evacuation"" src=""https://www.ecranlarge.com/media/cache/637x637/uploads/image/001/395/matrix-4-resurrections-photo-keanu-reeves-carrie-anne-moss-1395197.png"" alt=""Anti-hindrance emergency evacuation"" width=""466"" height=""195""></p>
<p style=""text-align:center""><em>A</em><em>nti - hindrance emergency evacuation</em></p>
<p>Despite some dubious comments (tight humor, etc.), it seems that we have escaped the unsuccessful reboot,<strong>to have a sequel that will really serve the saga</strong>, while plotting its own path. On the other hand, there is still some doubt as to the quantity of surprises that awaits us, and to what extent the staging will know how to reinvent itself. Are we going to enjoy our session? Certainly. Are we holding a cinema revolution(s)? Not sure.Is the prospect of a new saga possible following this film? Mystery.</p>",
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
