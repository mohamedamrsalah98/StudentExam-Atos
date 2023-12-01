import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Dashboard = () => {
  const [userCount, setUserCount] = useState(0);
  const [completedExamCount, setCompletedExamCount] = useState(0);
  const [passedExamCount, setPassedExamCount] = useState(0);
  const [failedExamCount, setFailedExamCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Fetch user count
        const usersResponse = await axios.get('https://localhost:7206/getUsersInRole?roleName=User');
        setUserCount(usersResponse.data.length);

        // Fetch exam results
        const examResultsResponse = await axios.get('https://localhost:7206/api/AllSubjectAllStudent/exam-results');
        const completedExams = examResultsResponse.data.filter(result => result.examTime !== null);
        const passedExams = completedExams.filter(result => result.score >= 50);

        setCompletedExamCount(completedExams.length);
        setPassedExamCount(passedExams.length);
        setFailedExamCount(completedExams.length - passedExams.length);
      } catch (error) {
        setError('Error fetching data');
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  return (
    <div>
      <h1>Dashboard</h1>
      {loading && <p>Loading...</p>}
      {error && <p>{error}</p>}
      {!loading && !error && (
        <table>
          <thead>
            <tr>
              <th>Category</th>
              <th>Count</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>User Count</td>
              <td>{userCount}</td>
            </tr>
            <tr>
              <td>Completed Exam Count</td>
              <td>{completedExamCount}</td>
            </tr>
            <tr>
              <td>Passed Exam Count</td>
              <td>{passedExamCount}</td>
            </tr>
            <tr>
              <td>Failed Exam Count</td>
              <td>{failedExamCount}</td>
            </tr>
          </tbody>
        </table>
      )}
    </div>
  );
};

export default Dashboard;
