#CGS: Community Geodesy System

*A geodesic grid system for Kerbal Space Program -- or anything else, I suppose.*

[logo]

##What is Geodesy?

**Geodesy** is a field of study dedicated to measuring and dividing up the Earth.  It's tougher than that definition makes it sound.  The Earth is not globally flat; rather, it's a lumpy spheroid, and that means that the simple mathematics of Euclidean geometry don't apply.  Even to this day, there is active research in figuring out better ways to capture and store data about the Earth (or other non-planar bodies) without distorting it.

##What is the Community Geodesy System?

The **Community Geodesy System** is a copylefted software library that solves the problems of geodesy and manages the tricky math involved.  At the moment, it only does so through a utility class, but more is coming Soon:TM: (see the Roadmap):

The CommunityGeodesyUtility class does a bunch of low-level number crunching that forms the basis of the other functions.  It can solve the direct geodetic problem (if I move a certain great-circle distance at a certain initial heading from a known position, where do I end up?) and indirect geodetic problem (given two points, how far apart are they, and what are the headings from one to the other?), as well as convert geographic coordinates to Cartesian coordinates and vice-versa.

This utility-class thing is actually doubly cool because it keeps the computation separate from any KSP interaction, so this software package can be used in stuff other than KSP addons, if such a need ever arose.

##Why do we need a Community Geodesy System?

We're in the business of doing work on spherical bodies, so we need to store data about them appropriately.  Rectangular maps are computationally convenient ways to store color, height, and other surface-based information, with easy adjacency, distance, and direction-pointing methods, but when applied to spheres, they become increasingly distorted at the poles and over large areas.  A geodesic grid is a much more robust structure for storing this kind of data.

##Why am I making a Community Geodesy System?

I first started playing Kerbal Space Program with version 0.23.5, when a few co-workers of mine put together a KSP challenge in their free time and invited me to join.  The contest organizer had done all the work of picking addons and such, so all I had to do was get the game and play.  This included Kethane, since the theme was to colonize other planets with ISRU and such.  I never got out of LKO during the challenge (it was one of my more experienced teammates who ended up doing most of the "real work"), but I was impressed by the slick hexagon maps that showed up on the opening screen and in the printouts that my teammate made when we picked our colony site.  I left KSP alone for a while after that, and by the time I got back into it, it was version 0.25 and I used Karbonite instead (and since), but I remember those hexmaps.  I've seen a couple of projects (and had a couple of ideas for other projects) that could benefit from a geodesic grid system, and Majiir's GeodesicGrid program is not permissively licensed, so I've decided to make a geodesy system for the community to use.

##Download and install

The source code is available on GitHub.  At the moment, there's nothing to install yet.  (Maybe there won't ever be anything to install on its own -- see "Roadmap".)

##Addons which use CGS

None so far, and I don't expect any until CGS is actually done.  Once it is, please let me know if your addon (or an addon you love) uses CGS and I'll list it here.  I've even got a "Powered by CGS" logo waiting for you!

[powered_by_cgs]

(I can make it say something other than "Powered by", if you think that would be better.)

##Version history

* 2015 XX: No release yet, just CommunityGeodesyUtility.

##Roadmap

At present, CGS just consists of CommunityGeodesyUtility, but I intend to build it all the way up to a full geodesic grid manager.  The CommunityGeodesyGrid class will create and store a set of CommunityGeodesyCells (of size, resolution and maybe initial orientation defined when calling the constructor).  It will contain methods to identify the grid cell occupied by a particular set of coordinates, and the CommunityGeodesyCell class will have methods to read out its center, vertices, and area, and give either the full set of a cell's neighbors or the neighbor which lies in a particular direction.  My vision is that plugin authors can use this framework to handle tricky adjacency stuff when doing operations like "move storm from cell X to the nearest cell in direction ABC".

There are still some HUUUUUGE knowledge gaps between here and there.  The big one is "how should my plugin-for-plugins work?"  Is this the sort of thing where I should compile a DLL and let mod authors...link into it somehow, like RemoteTech?  I feel like this could cause problems with inter-mod compatibility if I make changes (e.g. Mod A depends on behavior peculiar to version 1 of CGS, Mod B depends on something that was present in 1.1 but not 1.0), so I don't really want to do that.  On the other hand, I'm a little fuzzy on this whole "namespace" business (most of my programming experience is in scripting with MATLAB to support my engineering work) -- if two plugins have their own copies of CGS source files (different versions of CGS, say), and they get compiled, will each plugin carry and use "its own" CGS, or will there be a risk of them stepping on each other if both of their CGS files come from the same namespace?

More broadly, there's the means of storing the grid itself.  I've done the math on paper to generate the cells' centers and vertices, but I keep going back and forth on how the grid ought to be stored in software -- should I give each cell a name (I think I've figured out a scheme for giving a cell a name which encodes its position), and then give each cell a list of its neighbors' names, or should each cell just store a list of cells?  This is also related to how addons which use CGS will store their information -- is it easier to store that information as key-value pairs keyed by name, or can the cell object itself be the key?  (I know that, in general, anything can be a key, but I'm interested in what's easiest in the context of KSP.)  I'm an engineer, not a programmer.  I'm making this for the modder community, so if any of you all have input, I'd love to hear it!

After I get that sorted and released, I intend to make a sample program to show off the functionality of the CGS (and prove it actually works...).  It will create a CommunityGeodesyGrid and draw it on a planet (with an interface to show how the various grid options work, maybe highlight the cell the controlled vessel is in?).  This will be even further into the future since I will have to learn KSP interfacing stuff, though I'm studying Environmental Visual Enhancements to learn how to draw stuff on planets.

In the very long run, I've got a few other ideas for further applications of the grid system, which I'm tossing out in the event that anyone feels like picking them up:

1. Hexified map resource overlays.
2. Abstracted, board-game-like functionality.  It could be used for managing supply chains (e.g. mines, refineries, farms...), wargames, communication and recovery infrastructure...the possibilities are endless!
3. Weather and climate.  I know silverfox8124 and DaMichel have weather systems in the works, and I don't know how they are storing climate state data, but real climate modeling is done with geodesic grids, and using CGS will help avoid weird polar distortions.  CGS could also be used in systems that build on weather and climate, like pollution and terraforming (kerbiforming?).
4. A grid and node serializer/deserializer/ConfigNode-ifier system, so that maps can be saved and loaded as ConfigNodes instead of being recalculated.  A grid should be only be calculated once per game run per body anyway, but it might be nice to have the option to do it this way.  However, this also has ramifications for the name-as-key vs. cell-as-key question, which is why I'm mentioning this here.

##Anti-Roadmap

Adapting the CommunityGeodesyUtility functions to work for oblate spheroids is straightforward (although some of them have to be iterated until they converge to the desired accuracy, instead of being directly solvable), but the grid and nodes I'm not so sure about, and I'm not even going to think about taking that plunge while all KSP planets are spherical.

##Credits

I'd like to thank TriggerAu for his excellent "An Adventure in Plugin Coding" series and for excellent and permissively-licensed plugins and frameworks that I'm examining to get a feel for how KSP plugins fit together.  

I learned some of the more advanced in-source-documentation stuff from SCANSat, so thanks technogeeky and DMagic for making such a well-organized addon!

Many thanks to Thaddeus Vincenty for [**his formulae**](https://en.wikipedia.org/wiki/Vincenty%27s_formulae), and the editors of Wikipedia for their articles on said formulae and geodesic grids in general.

And of course, thanks are owed to Majiir for showing me what is possible and for giving me a reason to do it myself.

##License

The Community Geodesy System is copyright 2015 Kerbas_ad_astra and released under the GNU GPL v3 (or any later version).  If you make a fork (unless it's intended to be merged with the master or if I'm handing over central control to someone else), you must give it a different name in addition to the other anti-user-confusion provisions of the GPL (see sections 5a, 7).  All other rights reserved.

The Community Geodesy System logo is copyright 2015 Kerbas_ad_astra.  All rights reserved.  In the event that I hand off central control of CGS to anyone, I'll hand over the logo with it.

The "Powered by CGS" logo is copyright 2015 Kerbas_ad_astra.  Any program which uses the Community Geodesy System may include the "Powered by CGS" logo, unmodified, in its project homepage(s), preferably with a link back to the CGS project page on this forum, GitHub, or whatever CGS homepage(s) I make in the future.  All other rights reserved.

(My main reason for restricting rights to the name and logos is to avoid user confusion in the event of multiple CGS forks and extensions running around.  I doubt this will come to pass, but you never know.  I like making logos and icons, so I'm happy to help any fork or extension project that wants a logo of its own!  :))