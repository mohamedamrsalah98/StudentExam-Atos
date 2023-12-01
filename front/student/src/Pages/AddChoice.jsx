import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';

const AddChoice = () => {
  const [questions, setQuestions] = useState([]);
  const [selectedQuestion, setSelectedQuestion] = useState('');
  const [choiceText, setChoiceText] = useState('');
  const [isCorrect, setIsCorrect] = useState(false);
  const [choices, setChoices] = useState([]);

  useEffect(() => {
    // Fetch all questions
    const fetchQuestions = async () => {
      try {
        const response = await axios.get('https://localhost:7206/api/AddQuestion/GetAllQuestions');
        setQuestions(response.data);
      } catch (error) {
        console.error('Error fetching questions:', error);
      }
    };

    // Fetch choices for the selected question
    const fetchChoices = async () => {
      if (selectedQuestion) {
        try {
          const response = await axios.get(`https://localhost:7206/api/AddChoice/GetChoicesForQuestion/${selectedQuestion}`);
          setChoices(response.data);
        } catch (error) {
          console.error('Error fetching choices:', error);
        }
      }
    };

    fetchQuestions();
    fetchChoices();
  }, [selectedQuestion]);

  const handleAddChoice = async () => {
    try {
      if (!selectedQuestion || !choiceText) {
        toast.error('Please select a question and enter a choice text.');
        return;
      }

          // Check if there's already a correct choice for the selected question
    const hasCorrectChoice = choices.some((choice) => choice.isCorrect);
    if (isCorrect && hasCorrectChoice) {
      toast.error('There can only be one correct choice for the selected question.');
      return;
    }
      const response = await axios.post(`https://localhost:7206/api/AddChoice/${selectedQuestion}`, {
        choiceText,
        isCorrect,
      });

      toast.success('Choice added successfully:', response.data);

      // Fetch the updated list of choices after adding a new choice
      const updatedChoicesResponse = await axios.get(`https://localhost:7206/api/AddChoice/GetChoicesForQuestion/${selectedQuestion}`);
      setChoices(updatedChoicesResponse.data);

      // Clear the choice text and isCorrect state
      setChoiceText('');
      setIsCorrect(false);
    } catch (error) {
      console.error('Error adding choice:', error);
    }
  };

  return (
    <div>
      <h1>Add Choice for Question</h1>
      <label>
        Select Question:
        <select value={selectedQuestion} onChange={(e) => setSelectedQuestion(e.target.value)}>
          <option value="" disabled>Select a question</option>
          {questions.map((question) => (
            <option key={question.questionId} value={question.questionId}>
              {question.questionText}
            </option>
          ))}
        </select>
      </label>
      <br />
      <label>
        Enter Choice Text:
        <input type="text" value={choiceText} onChange={(e) => setChoiceText(e.target.value)} />
      </label>
      <br />
      <label>
        Is Correct:
        <input
          type="checkbox"
          checked={isCorrect}
          onChange={(e) => setIsCorrect(e.target.checked)}
        />
      </label>
      <br />
      <button onClick={handleAddChoice}>Add Choice</button>
      <br />
      {choices.length > 0 && (
        <div>
          <h2>Choices for the Selected Question</h2>
          <ul>
            {choices.map((choice) => (
              <li
                key={choice.choiceId}
                style={{ color: choice.isCorrect ? 'green' : 'red' }}

              >
                {choice.choiceText}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default AddChoice;
