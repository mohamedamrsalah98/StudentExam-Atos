import React, { useState, useEffect } from 'react';
import axios from 'axios';

const StudentScreen = () => {
  const [students, setStudents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(2); // Change this value based on your requirement

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axios.get('https://localhost:7206/getUsersInRole?roleName=User');
        setStudents(response.data);
      } catch (error) {
        setError('Error fetching students');
        console.error('Error fetching students:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchStudents();
  }, []);

  const handleStatusChange = async (userId, isActive, userName) => {
    try {
      // Ask for confirmation
      const confirmed = window.confirm(`Do you really want to toggle the status for ${userName}?`);
      if (!confirmed) {
        return;
      }

      await axios.put(`https://localhost:7206/StudentStatus/${userId}`, {
        isActive: !isActive, // Toggle the status
      });

      // Update the status locally
      setStudents((prevStudents) =>
        prevStudents.map((student) =>
          student.id === userId ? { ...student, isActive: !isActive } : student
        )
      );
    } catch (error) {
      console.error('Error changing student status:', error);
    }
  };

  // Pagination
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = students.slice(indexOfFirstItem, indexOfLastItem);

  // Change page
  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  return (
    <div>
      <h1>Student List</h1>
      {loading && <p>Loading...</p>}
      {error && <p>{error}</p>}
      {currentItems.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>Student Name</th>
              <th>Email</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {currentItems.map((student) => (
              <tr key={student.id}>
                <td>{student.userName}</td>
                <td>{student.email}</td>
                <td>{String(student.isActive)}</td>
                <td>
                  <button onClick={() => handleStatusChange(student.id, student.isActive, student.userName)}>
                    Toggle Status
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {students.length > itemsPerPage && (
        <div>
          {Array.from({ length: Math.ceil(students.length / itemsPerPage) }).map((_, index) => (
            <button key={index} onClick={() => paginate(index + 1)}>
              {index + 1}
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default StudentScreen;
