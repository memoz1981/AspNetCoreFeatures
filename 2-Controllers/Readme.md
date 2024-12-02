### This section covers notes and my own experimentation from book ASP.NET Core in Action (ANDREW LOCK)
**(Part 1, Section 4: Creating web pages with MVC controllers, MANNING SHELTER ISLAND, 
©2018 by Manning Publications Co., ISBN 9781617294617)**

**Controllers can be added/mapped in one of following ways**
- Deriving from Controller/ControllerBase (directly/indirectly)
- Class name should end with "Controller"
- Minimal APIs as additional method added @ later versions

**Important:**
- Class should be instantiable (not abstract etc.)
- Controller folder is optional just to be able to find easily
- Controllers are "discovered" at runtime