import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { toast } from 'react-toastify';

const Student = () => {
  const [subjects, setSubjects] = useState([]);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('https://localhost:7206/api/subjects');
        setSubjects(response.data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching data:', error);
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleSubjectChange = (event) => {
    setSelectedSubject(event.target.value);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const token = Cookies.get('token');
      const decodedToken = jwtDecode(token);
      const userid = decodedToken.uid;

      // Check if selectedSubject is empty before making the API request
      if (!selectedSubject) {
        toast.error('Selected subject is empty.');
        return; // Exit the function to prevent the API request
      }

      const { data } = await axios.post(
        `https://localhost:7206/api/studentsubjects/${userid}`,
        { subjectName: selectedSubject }
      );
      toast.success(data);
    } catch (error) {
      if (error.response && error.response.data) {
        toast.error(error.response.data);
      } else {
        console.error('Unexpected error:', error);
        toast.error('An unexpected error occurred');
      }
    }
  };

  return (
    <div>
      <h1>Subject Form</h1>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <form onSubmit={handleSubmit}>
          <br />
          <label>
            Select Subject:
            <select value={selectedSubject} onChange={handleSubjectChange}>
              <option value="" disabled>Select a subject</option>
              {subjects.map((subject) => (
                <option key={subject.id} value={subject.subjectName}>
                  {subject.subjectName}
                </option>
              ))}
            </select>
          </label>
          <br />
          <button type="submit">Submit</button>
        </form>
      )}
    </div>
  );
};

export default Student;
