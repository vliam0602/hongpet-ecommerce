import { useState, useEffect } from 'react';
import { Eye } from 'lucide-react';
import customerService from "../../services/customerService";

function Customers() {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [pageIndex, setPageIndex] = useState(1); // Current page
  const [pageSize] = useState(10); // Items per page
  const [totalPages, setTotalPages] = useState(1); // Total pages

  useEffect(() => {
    const fetchCustomers = async () => {
      setLoading(true);
      try {
        const data = await customerService.getCustomers(pageIndex, pageSize);
        setCustomers(data.items); // Items from PagedList
        setTotalPages(data.totalPages); // Total pages from PagedList
      } catch (error) {
        console.error('Error fetching customers:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCustomers();
  }, [pageIndex, pageSize]);

  const handlePageChange = (page) => {
    setPageIndex(page);
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-full">
        Loading customers...
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Customers</h1>
      </div>

      <div className="card">
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th className="hidden">Id</th>
                <th>Name</th>
                <th>Email</th>
                <th>Username</th>
                <th>Total Orders</th>
                <th>Total Spend</th>
                <th>Created Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {customers.length > 0 ? (
                customers.map((customer) => (
                  <tr key={customer.id}>
                    <td className="hidden font-medium">{customer.id}</td>
                    <td className="font-medium">{customer.fullname}</td>
                    <td className="font-medium">{customer.email}</td>
                    <td className="font-medium">{customer.username}</td>
                    <td className="font-medium">{customer.totalOrders}</td>
                    <td className="font-medium">
                      {new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(customer.totalSpend)}
                    </td>
                    <td className="font-medium">
                      {new Date(customer.createdDate).toLocaleDateString()}
                    </td>
                    <td>
                      <div className="flex items-center">
                        <button className="p-1 text-blue-600 hover:text-blue-800">
                          <Eye size={18} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="text-center py-4">
                    No customers found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      {/* Pagination Controls */}
      <div className="pagination-container flex justify-center mt-4">
        <nav aria-label="Page navigation">
          <ul className="pagination flex space-x-2">
            {/* Previous Page */}
            {pageIndex > 1 && (
              <li className="page-item">
                <button
                  className="page-link px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
                  onClick={() => handlePageChange(pageIndex - 1)}
                >
                  Previous
                </button>
              </li>
            )}

            {/* Page Numbers */}
            {Array.from({ length: totalPages }, (_, i) => i + 1).map((page) => (
              <li
                key={page}
                className={`page-item ${
                  page === pageIndex ? "bg-primary text-white" : "bg-gray-200"
                } rounded`}
              >
                <button
                  className="page-link px-4 py-2 hover:bg-accent rounded"
                  onClick={() => handlePageChange(page)}
                >
                  {page}
                </button>
              </li>
            ))}

            {/* Next Page */}
            {pageIndex < totalPages && (
              <li className="page-item">
                <button
                  className="page-link px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
                  onClick={() => handlePageChange(pageIndex + 1)}
                >
                  Next
                </button>
              </li>
            )}
          </ul>
        </nav>
      </div>
    </div>
  );
}

export default Customers;