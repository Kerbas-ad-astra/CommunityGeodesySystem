* Class CommunityGeodesyUtility (static utility methods):
	* Forward solver (point + heading + distance -> destination) (ForwardGeoSolve)
	* Inverse solver (two points -> distance and headings) (InverseGeoSolve)
	* Lat/Long <-> Cartesian (LatLon2Cart, Cart2LatLon)
* Class CommuntiyGeodesyGrid, has-a Radius, Resolution (max size of cell -> subdivisions = ceiling(Radius/Resolution) or something), North Pole (tuple of coordinates where the primary vertex goes), Prime Meridian (which direction the first edge goes), collection of CommunityGeodesyCells, "drawing factor" to ensure that lines and such get drawn on top of the surface (as opposed to inside it).  Hex/penta-grid based on icosahedron.
	* On construction, create a new grid by defining icosahedral vertices, subdividing, and projecting.  Vertices become centers of nodes, centroids of triangles become vertices of the boundary.
	* Geographic coordinates -> Cell (GetCellFromLatLon)
* Class CommunityGeodesyCell: has-a name?, center (vec3d, get), vertices (collection of vec3ds, get), neighbors (collection of CGCells or names?, get), area (calculated and stored on creation, get), nearest-neighbor-in-direction (GetNeighborInHeading).
* Think about: where can we use floats instead of doubles?