-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObtenerProductos
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT Producto.Id, Producto.IdSubCategoria, Producto.Nombre, Producto.Descripcion, Producto.Precio, Producto.Stock, Producto.CodigoBarras, Categorias.Nombre AS Categoria, SubCategorias.Nombre AS SubCategoria
FROM     Producto INNER JOIN
                  SubCategorias ON Producto.IdSubCategoria = SubCategorias.Id INNER JOIN
                  Categorias ON SubCategorias.IdCategoria = Categorias.Id
END