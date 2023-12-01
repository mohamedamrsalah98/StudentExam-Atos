import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';

const AddExam = () => {
  const [examTitle, setExamTitle] = useState('');
  const [duration, setDuration] = useState('');
  const [selectedQuestions, setSelectedQuestions] = useState([]);
  const [questions, setQuestions] = useState([]);
  const [subjects, setSubjects] = useState([]);
  const [selectedSubject, setSelectedSubject] = useState('');

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const response = await axios.get('https://localhost:7206/api/subjects');
        setSubjects(response.data);
      } catch (error) {
        console.error('Error fetching subjects:', error);
      }
    };

    fetchSubjects();
  }, []);
  
  useEffect(() => {
    const fetchQuestions = async () => {
      if (selectedSubject) {
        try {
          const selectedSubjectObj = subjects.find(subject => subject.subjectName === selectedSubject);

          if (selectedSubjectObj) {
            const response = await axios.get(`https://localhost:7206/api/AddQuestion/GetQuestionsForSubject/${selectedSubjectObj.id}`);
            setQuestions(response.data);
          }
        } catch (error) {
          console.error('Error fetching questions:', error);
        }
      }
    };

    fetchQuestions();
  }, [selectedSubject, subjects]);

  const handleAddExam = async () => {
    try {
      if (!examTitle || !duration || selectedQuestions.length === 0 || !selectedSubject) {
        toast.error('Please fill in all the fields and select at least one question and a subject.');
        return;
      }
  
      const selectedSubjectObj = subjects.find(subject => subject.subjectName === selectedSubject);
  
      if (selectedSubjectObj) {
        const payload = {
          examTitle,
          duration,
          questionIds: selectedQuestions,
        };
  
        const response = await axios.post(`https://localhost:7206/api/AddExam/${selectedSubjectObj.id}`, payload);
        toast.success(response.data);
  
        setExamTitle('');
        setDuration('');
        setSelectedQuestions([]);
        setSelectedSubject('');
      }
    } catch (error) {
      toast.error('Error adding exam:', error);
    }
  };

  const handleCheckboxChange = (questionId) => {
    setSelectedQuestions(prevQuestions => {
      if (prevQuestions.includes(questionId)) {
        return prevQuestions.filter((id) => id !== questionId);
      } else {
        return [...prevQuestions, questionId];
      }
    });
  };

  return (
    <div>
      <h1>Add Exam</h1>
      <label>
        Select Subject:
        <select value={selectedSubject} onChange={(e) => setSelectedSubject(e.target.value)}>
          <option value="" disabled>Select a subject</option>
          {subjects.map((subject) => (
            <option key={subject.id} value={subject.subjectName}>
              {subject.subjectName}
            </option>
          ))}
        </select>
      </label>
      <br />
      <label>
        Exam Title:
        <input type="text" value={examTitle} onChange={(e) => setExamTitle(e.target.value)} />
      </label>
      <br />
      <label>
        Duration:
        <input type="text" value={duration} onChange={(e) => setDuration(e.target.value)} />
      </label>
      <br />
      {questions.length > 0 && (
        <div>
          <h2>Questions for the Subject</h2>
          <ul>
            {questions.map((question) => (
              <li key={question.questionId}>
                <input
                  type="checkbox"
                  checked={selectedQuestions.includes(question.questionId)}
                  onChange={() => handleCheckboxChange(question.questionId)}
                />
                {question.questionText}
              </li>
            ))}
          </ul>
        </div>
      )}
      <br />
      <button onClick={handleAddExam}>Add Exam</button>
    </div>
  );
};

export default AddExam;
