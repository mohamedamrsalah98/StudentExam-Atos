import React, { useState, useEffect } from 'react';
import axios from 'axios';

const ExamScreen = () => {
  const [data, setData] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(2); 
  const [subjectFilter, setSubjectFilter] = useState('');
  const [studentNameFilter, setStudentNameFilter] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('https://localhost:7206/api/AllSubjectAllStudent/student-exam-info');
        setData(response.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;

  const filteredData = data
    .filter(item => item.subjectName.toLowerCase().includes(subjectFilter.toLowerCase()))
    .filter(item => item.studentName.toLowerCase().includes(studentNameFilter.toLowerCase()));

  const currentItems = filteredData.slice(indexOfFirstItem, indexOfLastItem);

  return (
    <div>
      <h1>All Subject All Student Table</h1>
      <div>
        <label>
          Subject Filter:
          <input type="text" value={subjectFilter} onChange={(e) => setSubjectFilter(e.target.value)} />
        </label>
        <label>
          Student Name Filter:
          <input type="text" value={studentNameFilter} onChange={(e) => setStudentNameFilter(e.target.value)} />
        </label>
      </div>
      
      {filteredData.length === 0 ? (
        <p>There is no student taking the exam yet.</p>
      ) : (
        <>
          <table>
            <thead>
              <tr>
                <th>Student Name</th>
                <th>Subject Name</th>
                <th>Score</th>
                <th>Date Time Of Exam</th>
              </tr>
            </thead>
            <tbody>
              {currentItems.map((item, index) => (
                <tr key={index}>
                  <td>{item.studentName}</td>
                  <td>{item.subjectName}</td>
                  <td>{item.score}</td>
                  <td>{item.dateTimeOfExam}</td>
                </tr>
              ))}
            </tbody>
          </table>
          <div>
            {Array.from({ length: Math.ceil(filteredData.length / itemsPerPage) }).map((_, index) => (
              <button key={index} onClick={() => setCurrentPage(index + 1)}>
                {index + 1}
              </button>
            ))}
          </div>
        </>
      )}
    </div>
  );
};

export default ExamScreen;
