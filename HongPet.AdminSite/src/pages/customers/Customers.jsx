import { useState, useEffect } from 'react'
import { Search, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'

// Mock data for customers
const mockCustomers = [
  {
    id: '1',
    name: 'John Doe',
    email: 'john.doe@example.com',
    phone: '(123) 456-7890',
    address: '123 Main St, City, Country',
    totalOrders: 12,
    totalSpent: 1250.75,
    createdDate: '2025-01-15'
  },
  {
    id: '2',
    name: 'Jane Smith',
    email: 'jane.smith@example.com',
    phone: '(234) 567-8901',
    address: '456 Oak Ave, Town, Country',
    totalOrders: 8,
    totalSpent: 875.50,
    createdDate: '2025-02-20'
  },
  {
    id: '3',
    name: 'Robert Johnson',
    email: 'robert.johnson@example.com',
    phone: '(345) 678-9012',
    address: '789 Pine Rd, Village, Country',
    totalOrders: 5,
    totalSpent: 450.25,
    createdDate: '2025-03-10'
  },
  {
    id: '4',
    name: 'Emily Davis',
    email: 'emily.davis@example.com',
    phone: '(456) 789-0123',
    address: '101 Maple Dr, Suburb, Country',
    totalOrders: 15,
    totalSpent: 1875.00,
    createdDate: '2025-01-05'
  },
  {
    id: '5',
    name: 'Michael Wilson',
    email: 'michael.wilson@example.com',
    phone: '(567) 890-1234',
    address: '202 Cedar Ln, District, Country',
    totalOrders: 3,
    totalSpent: 225.75,
    createdDate: '2025-04-25'
  },
]

function Customers() {
  const [customers, setCustomers] = useState([])
  const [loading, setLoading] = useState(true)
  const [searchTerm, setSearchTerm] = useState('')
  const [sortField, setSortField] = useState('name')
  const [sortDirection, setSortDirection] = useState('asc')
  
  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setCustomers(mockCustomers)
      setLoading(false)
    }, 500)
  }, [])
  
  const handleSort = (field) => {
    if (sortField === field) {
      setSortDirection(sortDirection === 'asc' ? 'desc' : 'asc')
    } else {
      setSortField(field)
      setSortDirection('asc')
    }
  }
  
  const filteredCustomers = customers.filter(customer => 
    customer.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    customer.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
    customer.phone.includes(searchTerm)
  )
  
  const sortedCustomers = [...filteredCustomers].sort((a, b) => {
    let aValue = a[sortField]
    let bValue = b[sortField]
    
    if (typeof aValue === 'string') {
      aValue = aValue.toLowerCase()
      bValue = bValue.toLowerCase()
    }
    
    if (aValue < bValue) return sortDirection === 'asc' ? -1 : 1
    if (aValue > bValue) return sortDirection === 'asc' ? 1 : -1
    return 0
  })
  
  if (loading) {
    return <div className="flex justify-center items-center h-full">Loading customers...</div>
  }
  
  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Customers</h1>
      </div>
      
      <div className="card">
        <div className="mb-6">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Search customers..."
              className="input-field pl-10"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
        </div>
        
        <div className="table-container">
          <table className="table">
            <thead>
              <tr>
                <th 
                  className="cursor-pointer hover:bg-gray-100"
                  onClick={() => handleSort('name')}
                >
                  <div className="flex items-center">
                    Name
                    {sortField === 'name' && (
                      <span className="ml-1">{sortDirection === 'asc' ? '↑' : '↓'}</span>
                    )}
                  </div>
                </th>
                <th className="hidden md:table-cell">Contact</th>
                <th 
                  className="hidden md:table-cell cursor-pointer hover:bg-gray-100"
                  onClick={() => handleSort('totalOrders')}
                >
                  <div className="flex items-center">
                    Orders
                    {sortField === 'totalOrders' && (
                      <span className="ml-1">{sortDirection === 'asc' ? '↑' : '↓'}</span>
                    )}
                  </div>
                </th>
                <th 
                  className="hidden md:table-cell cursor-pointer hover:bg-gray-100"
                  onClick={() => handleSort('totalSpent')}
                >
                  <div className="flex items-center">
                    Total Spent
                    {sortField === 'totalSpent' && (
                      <span className="ml-1">{sortDirection === 'asc' ? '↑' : '↓'}</span>
                    )}
                  </div>
                </th>
                <th 
                  className="hidden md:table-cell cursor-pointer hover:bg-gray-100"
                  onClick={() => handleSort('createdDate')}
                >
                  <div className="flex items-center">
                    Joined
                    {sortField === 'createdDate' && (
                      <span className="ml-1">{sortDirection === 'asc' ? '↑' : '↓'}</span>
                    )}
                  </div>
                </th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {sortedCustomers.length > 0 ? (
                sortedCustomers.map((customer) => (
                  <tr key={customer.id}>
                    <td>
                      <div className="font-medium">{customer.name}</div>
                      <div className="text-sm text-gray-500 md:hidden">{customer.email}</div>
                      <div className="text-sm text-gray-500 md:hidden">{customer.phone}</div>
                    </td>
                    <td className="hidden md:table-cell">
                      <div>{customer.email}</div>
                      <div className="text-sm text-gray-500">{customer.phone}</div>
                    </td>
                    <td className="hidden md:table-cell">{customer.totalOrders}</td>
                    <td className="hidden md:table-cell">${customer.totalSpent.toFixed(2)}</td>
                    <td className="hidden md:table-cell">{customer.createdDate}</td>
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
                  <td colSpan="6" className="text-center py-4">No customers found</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  )
}

export default Customers