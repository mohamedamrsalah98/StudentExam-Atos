import './App.css';
import { BrowserRouter, Route, Switch } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import SignUp from './Pages/signup';
import Login from './Pages/login';
import { AuthProvider } from './Context/AuthContext';
import Student from './Pages/Student';
import Navbar from './components/Navbar';
import StartExam from './Pages/StartExam';
import ExamResult from './Pages/ExamResult';
import AddQuestion from './Pages/AddQuestions';
import AddChoice from './Pages/AddChoice';
import AddExam from './Pages/AddExam';
import ExamScreen from './Pages/ExamsScreen';
import StudentScreen from './Pages/StudentScreen';
import Dashboard from './Pages/Dashboard';
import SignalR from './components/SignalR';



function App() {
  return (
    <div className="App">
      <ToastContainer />
      <AuthProvider>
        <BrowserRouter>
          <Navbar />
          <Switch>

          <Route exact path={"/"} component={Login} /> 
          <Route exact path={"/signup"} component={SignUp} />

          <Route exact path={"/student"} component={Student} />
          <Route exact path={"/startexam"} component={StartExam} />
          <Route exact path={"/examresult"} component={ExamResult} />
          
          <Route exact path={"/addexam"} component={AddExam} />
          <Route exact path={"/addquestion"} component={AddQuestion} />
          <Route exact path={"/addchoice"} component={AddChoice} />
          <Route exact path={"/examscreen"} component={ExamScreen} />
          <Route exact path={"/studentscreen"} component={StudentScreen} />
          <Route exact path={"/dashboard"} component={Dashboard} />
          <Route exact path={"/signalR"} component={SignalR} />



          </Switch>
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
}

export default App;
