import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

const StartExam = () => {
  const history = useHistory();

  const [subjects, setSubjects] = useState([]);
  const [selectedSubjectId, setSelectedSubjectId] = useState('');
  const [exam, setExam] = useState(null);
  const [selectedChoices, setSelectedChoices] = useState({});
  const [error, setError] = useState(null);
  const [time, setTime] = useState({ hours: 0, minutes: 0, seconds: 0 });
  const [examSubmitted, setExamSubmitted] = useState(false);

  //const takenSubjects = JSON.parse(localStorage.getItem('takenSubjects')) || [];

  const token = Cookies.get('token');
  const decodedToken = jwtDecode(token);
  const userid = decodedToken.uid;

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const response = await axios.get(`https://localhost:7206/api/studentsubjects/${userid}`);
        setSubjects(response.data);
        setError(null);
      } catch (error) {
        setError('Error fetching subjects. Please try again.');
        setSubjects([]);
      }
    };

    fetchSubjects();
  }, []);

  useEffect(() => {
    if (exam && exam.duration) {
      const durationInSeconds = convertTimeSpanToSeconds(exam.duration);
      setTime({
        hours: Math.floor(durationInSeconds / 3600),
        minutes: Math.floor((durationInSeconds % 3600) / 60),
        seconds: durationInSeconds % 60,
      });
    }
  }, [exam]);

  useEffect(() => {
    const intervalId = setInterval(() => {
      if (time.hours === 0 && time.minutes === 0 && time.seconds === 0) {
        clearInterval(intervalId);
        if (!examSubmitted) {
          handleSubmitExam();
        }
      } else {
        updateTime();
      }
    }, 1000);

    return () => clearInterval(intervalId);
  }, [time, examSubmitted]);

  const convertTimeSpanToSeconds = (timeSpanString) => {
    const [hours, minutes, seconds] = timeSpanString.split(':').map(Number);
    return hours * 3600 + minutes * 60 + seconds;
  };

  const updateTime = () => {
    if (time.minutes === 0 && time.seconds === 0) {
      setTime({ hours: time.hours - 1, minutes: 59, seconds: 59 });
    } else if (time.seconds === 0) {
      setTime({ ...time, minutes: time.minutes - 1, seconds: 59 });
    } else {
      setTime({ ...time, seconds: time.seconds - 1 });
    }
  };

  const handleSubjectChange = (event) => {
    setSelectedSubjectId(event.target.value);

    const defaultChoices = {};
    if (exam && exam.questions) {
      exam.questions.forEach((question) => {
        defaultChoices[question.questionId] = 0;
      });
    }

    setSelectedChoices(defaultChoices);
  };

  const handleGetExam = async () => {
    try {
      // if (takenSubjects.includes(selectedSubjectId)) {
      //   return toast.error('You have already tested this subject');
      // }

      const response = await axios.post('https://localhost:7206/api/GetExam/random', {
        studentId: userid,
        subjectId: selectedSubjectId,
      });
      setExam(response.data);
      setError(null);
      setExamSubmitted(false);

      //const updatedTakenSubjects = [...takenSubjects, selectedSubjectId];
      //localStorage.setItem('takenSubjects', JSON.stringify(updatedTakenSubjects));
    } catch (error) {
      setError(error.message || 'Error fetching exam data. Please check the subject and student IDs.');
      setExam(null);
    }
  };

  const handleChoiceChange = (questionId, choiceId) => {
    setSelectedChoices({
      ...selectedChoices,
      [questionId]: choiceId,
    });
  };

  const handleSubmitExam = async () => {
    try {
      if (!exam) {
        return;
      }
      const response = await axios.post('https://localhost:7206/api/GetExam/submit', {
        examId: exam.examId,
        studentId: userid,
        choices: Object.entries(selectedChoices).map(([questionId, selectedChoiceId]) => ({
          questionId,
          selectedChoiceId,
        })),
      });
      toast.success('Exam submitted successfully');
      setExamSubmitted(true);
      history.push('/examresult');
    } catch (error) {
      setError(error.message || 'Error submitting the exam. Please try again.');
      console.error('Error submitting the exam:', error);
    }
  };

  return (
    <div>
      <label htmlFor="subjectId">Select Subject: </label>
      <select id="subjectId" onChange={handleSubjectChange} value={selectedSubjectId}>
        <option value="">Select a subject</option>
        {subjects.map((subject) => (
          <option key={subject.id} value={subject.id}>
            {subject.subjectName}
          </option>
        ))}
      </select>
      <button onClick={handleGetExam}>Get Exam</button>

      {error && <div style={{ color: 'red' }}>{error}</div>}

      {exam && (
        <div>
          <h2>{exam.examTitle}</h2>
          {exam && !examSubmitted && (
        <div>
          <h4>Duration</h4>
          <p>{`${String(time.hours).padStart(2, '0')}:${String(time.minutes).padStart(2, '0')}:${String(
            time.seconds
          ).padStart(2, '0')}`}</p>
        </div>
      )}

          <h3>Questions:</h3>
          {exam.questions.map((question) => (
            <div key={question.questionId}>
              <p>{question.questionText}</p>
              <ul>
                {question.choices.map((choice) => (
                  <li key={choice.choiceId}>
                    <input
                      type="radio"
                      id={`choice_${choice.choiceId}`}
                      name={`question_${question.questionId}`}
                      value={choice.choiceId}
                      checked={selectedChoices[question.questionId] === choice.choiceId}
                      onChange={() => handleChoiceChange(question.questionId, choice.choiceId)}
                    />
                    <label htmlFor={`choice_${choice.choiceId}`}>{choice.choiceText}</label>
                  </li>
                ))}
              </ul>
            </div>
          ))}

          <button onClick={handleSubmitExam}>Submit Exam</button>
        </div>
      )}


    </div>
  );
};

export default StartExam;
