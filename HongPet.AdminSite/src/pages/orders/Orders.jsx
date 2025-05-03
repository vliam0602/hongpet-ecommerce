import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Search, Eye } from 'lucide-react'
import orderService from '../../services/orderService'

function Orders() {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState('all')
  const [pagination, setPagination] = useState({
    pageIndex: 1,
    pageSize: 10,
    totalItems: 0,
    totalPages: 0
  })
  
  useEffect(() => {
    const fetchOrders = async () => {
      setLoading(true)
      try {
        const data = await orderService.getOrders(
          pagination.pageIndex, 
          pagination.pageSize, 
          searchTerm
        )
        setOrders(data.items)
        setPagination({
          pageIndex: data.currentPage,
          pageSize: data.pageSize,
          totalItems: data.totalCount,
          totalPages: data.totalPages
        })
        setError(null)
      } catch (err) {
        setError('Failed to fetch orders. Please try again later.')
        console.error('Error fetching orders:', err)
      } finally {
        setLoading(false)
      }
    }
    
    // Debounce search input
    const timer = setTimeout(() => {
      fetchOrders()
    }, 500)
    
    return () => clearTimeout(timer)
  }, [searchTerm, pagination.pageIndex, pagination.pageSize])
  
  const filteredOrders = orders.filter(order => {
    if (statusFilter === 'all') return true;
    return order.status.toLowerCase() === statusFilter.toLowerCase();
  })
  
  const getStatusBadgeClass = (status) => {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'badge-success'
      case 'processing':
        return 'badge-warning'
      case 'pending':
        return 'badge-warning'
      case 'cancelled':
        return 'badge-danger'
      default:
        return 'bg-gray-100 text-gray-800'
    }
  }
  
  if (loading && orders.length === 0) {
    return <div className="flex justify-center items-center h-full">Loading orders...</div>
  }
  
  if (error) {
    return <div className="flex justify-center items-center h-full text-red-500">{error}</div>
  }
  
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Orders</h1>
      </div>
      
      <div className="card">
        <div className="flex flex-col md:flex-row gap-4 mb-6">
          {/* Search Box */}
          <div className="relative flex-1">
            <Search className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Search orders..."
              className="input-field pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          {/* Filter by status buttons */}
          <div className="flex flex-wrap gap-2">
            <button 
              className={`btn ${statusFilter === 'all' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('all')}
            >
              All
            </button>
            <button 
              className={`btn ${statusFilter === 'pending' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('pending')}
            >
              Pending
            </button>
            <button 
              className={`btn ${statusFilter === 'processing' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('processing')}
            >
              Processing
            </button>
            <button 
              className={`btn ${statusFilter === 'completed' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('completed')}
            >
              Completed
            </button>
            <button 
              className={`btn ${statusFilter === 'cancelled' ? 'btn-primary' : 'btn-dark'}`}
              onClick={() => setStatusFilter('cancelled')}
            >
              Cancelled
            </button>
          </div>
        </div>
        
        {loading && orders.length > 0 && 
        <div className="text-center py-4">Refreshing data...</div>}
        
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th className="hidden">Order ID</th>
                <th>Customer</th>
                <th className="hidden md:table-cell">Date</th>
                <th className="hidden md:table-cell">Total</th>
                <th>Status</th>
                <th className="hidden md:table-cell">Payment</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredOrders.length > 0 ? (
                filteredOrders.map((order) => (
                  <tr key={order.id}>
                    <td className="hidden">{order.id}</td>
                    <td>
                      <div className="font-medium">{order.customerName}</div>
                      <div className="text-sm text-gray-500">{order.customerPhone}</div>
                    </td>
                    <td className="hidden md:table-cell">
                      {new Date(order.createdDate).toLocaleString()}
                    </td>
                    <td className="hidden md:table-cell">
                      {new Intl.NumberFormat('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      }).format(order.totalAmount)}
                    </td>
                    <td>
                      <span className={`badge ${getStatusBadgeClass(order.status)}`}>
                        {order.status}
                      </span>
                    </td>
                    <td className="hidden md:table-cell">{order.paymentMethod}</td>
                    <td>
                      <Link to={`/orders/${order.id}`} className="p-1 text-blue-600 hover:text-blue-800">
                        <Eye size={18} />
                      </Link>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="text-center py-4">No orders found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        
        {/* Pagination */}
        <div className="flex justify-between items-center mt-4">
            <div>
              Showing {(pagination.pageIndex - 1) * pagination.pageSize + 1} to {Math.min(pagination.pageIndex * pagination.pageSize, pagination.totalItems)} of {pagination.totalItems} entries
            </div>
            <div className="flex gap-2">
              {pagination.pageIndex > 1 && (
                <button 
                  className="btn btn-dark"
                  onClick={() => setPagination(prev => ({ ...prev, pageIndex: prev.pageIndex - 1 }))}
                >
                  Previous
                </button>
              )}
              {pagination.pageIndex < pagination.totalPages && (
                <button
                className="btn btn-dark"
                onClick={() => setPagination(prev => ({ ...prev, pageIndex: prev.pageIndex + 1 }))}
                >
                  Next
                </button>
              )}
            </div>
        </div>
      
      </div>
    </div>
  )
}

export default Orders