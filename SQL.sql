--Obtener la lista de los productos no descatalogados incluyendo el nombre de la categoría ordenado por nombre de producto.
select P.ProductID, P.ProductName, C.CategoryName 
from Products p
inner join Categories c on (P.CategoryID = C.CategoryID)
where
	P.discontinued = 0
order by P.ProductName


--Mostrar el nombre de los clientes de Nancy Davolio.
select distinct C.ContactName
from Orders O
Inner join Customers C on (O.CustomerID = C.CustomerID) 
where
O.EmployeeID = 1


--Mostrar el total facturado por año del empleado Steven Buchanan.
select E.FirstName, sum(O.Freight) as Total_Ventas, Year(O.OrderDate) as Fecha
from Employees E
inner join Orders O on (E.EmployeeID = O.EmployeeID)
where
E.EmployeeID = 5
Group By Year(O.OrderDate), E.FirstName


--Mostrar el nombre de los empleados que dependan directa o indirectamente de Andrew Fuller.
select * from Employees
where
ReportsTo IS NOT NULL

