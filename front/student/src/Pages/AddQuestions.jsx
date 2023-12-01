import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';

const AddQuestion = () => {
  const [subjects, setSubjects] = useState([]);
  const [selectedSubjectId, setSelectedSubjectId] = useState('');
  const [questionText, setQuestionText] = useState('');
  const [questions, setQuestions] = useState([]);

  useEffect(() => {
    // Fetch all subjects
    const fetchSubjects = async () => {
      try {
        const response = await axios.get('https://localhost:7206/api/subjects');
        setSubjects(response.data);
      } catch (error) {
        console.error('Error fetching subjects:', error);
      }
    };

    // Fetch questions for the selected subject
    const fetchQuestions = async () => {
      if (selectedSubjectId) {
        try {
          const response = await axios.get(`https://localhost:7206/api/AddQuestion/GetQuestionsForSubject/${selectedSubjectId}`);
          setQuestions(response.data);
        } catch (error) {
          console.error('Error fetching questions:', error);
        }
      }
    };

    fetchSubjects();
    fetchQuestions();
  }, [selectedSubjectId]);

  const handleAddQuestion = async () => {
    try {
      if (!selectedSubjectId || !questionText) {
        toast.error('Please select a subject and enter a question text.');
        return;
      }

      const response = await axios.post(`https://localhost:7206/api/AddQuestion/${selectedSubjectId}`, {
        questionText,
      });

      toast.success('Question added successfully:', response.data);
      
      // Fetch the updated list of questions after adding a new question
      const updatedQuestionsResponse = await axios.get(`https://localhost:7206/api/AddQuestion/GetQuestionsForSubject/${selectedSubjectId}`);
      setQuestions(updatedQuestionsResponse.data);

      // Clear the question text
      setQuestionText('');
    } catch (error) {
      console.error('Error adding question:', error);
    }
  };

  return (
    <div>
      <h1>Add Question for Subject</h1>
      <label>
        Select Subject:
        <select value={selectedSubjectId} onChange={(e) => setSelectedSubjectId(e.target.value)}>
          <option value="" disabled>Select a subject</option>
          {subjects.map((subject) => (
            <option key={subject.id} value={subject.id}>
              {subject.subjectName}
            </option>
          ))}
        </select>
      </label>
      <br />
      <label>
        Enter Question Text:
        <input type="text" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
      </label>
      <br />
      <button onClick={handleAddQuestion}>Add Question</button>
      <br />
      {questions.length > 0 && (
        <div>
          <h2>Questions for the Selected Subject</h2>
          <ul>
            {questions.map((question) => (
              <li key={question.questionId}>{question.questionText}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default AddQuestion;
