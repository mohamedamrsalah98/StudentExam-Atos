import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';

const ExamResult = () => {
  const [studentExams, setStudentExams] = useState([]);
  const [error, setError] = useState(null);

  const token = Cookies.get('token');
  const decodedToken = jwtDecode(token);
  const userid = decodedToken.uid;

  useEffect(() => {
    const fetchStudentExams = async () => {
      try {

        const response = await axios.get(`https://localhost:7206/api/GetExam/get-student-exams/${userid}`);
        setStudentExams(response.data);
        setError(null);
      } catch (error) {
        setError('Error fetching student exams. Please try again.');
        setStudentExams([]);
      }
    };

    fetchStudentExams();
  }, []);

  return (
    <div>
      <h2>Exams Results</h2>
      {error && <div style={{ color: 'red' }}>{error}</div>}
      <ul>
        {studentExams.map((exam) => (
          <li key={exam.examId}>
            <p>Exam Title: {exam.examTitle}</p>
            <p>Exam Date and Time: {exam.examTime}</p>
            <p>Score: {exam.score}</p>
            <p>Subject: {exam.subjectTitle}</p>


            {/* Add more details as needed */}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ExamResult;
