import React, { useState } from "react";
import { Route, Routes } from "react-router-dom";
import "./App.css";
import debug from "sabio-debug";

import Home from "./components/Generic/Home";
import Footer from "./components/Generic/Footer";
import TestAndAjax from "./components/Generic/TestAndAjax";
import SiteNav from "./components/Generic/SiteNav";
import FriendForm from "./components/Friends/FriendForm";
import Friends from "./components/Friends/Friends";
import Events from "./components/Events/Events";
import Jobs from "./components/Jobs/Jobs";
import Companies from "./components/TechCompanies/Companies";
import Login from "./components/User/Login";
import Register from "./components/User/Register";
import PoliticalCandidates from "./components/codeChallenge/PoliticalCandidates";
import People from "./Lectures/MappingFiltering";
import Cars from "./components/codeChallenge/Cars";
import Basic from "./components/Basic";

function App() {
  const _logger = debug.extend("App");
  const [currentUser] = useState({
    firstName: "Josh",
    lastName: "Haynes",
    isLoggedIn: true,
  });
  _logger("Current User is", currentUser);
  //console.log(currentUser, setCurrentUser);
  return (
    <React.Fragment>
      <SiteNav
        firstName={currentUser.firstName}
        lastName={currentUser.lastName}
      ></SiteNav>

      <div className="route-container">
        <Routes>
          <Route path="/" element={<Home user={currentUser}></Home>}></Route>
          <Route path="/friends/new" element={<FriendForm />}></Route>
          <Route path="/friends/:friendId" element={<FriendForm />}></Route>
          <Route path="/friends" element={<Friends />}></Route>
          <Route path="/events" element={<Events />}></Route>
          <Route path="/jobs" element={<Jobs />}></Route>
          <Route path="/companies" element={<Companies />}></Route>
          <Route path="/testandajax" element={<TestAndAjax />}></Route>
          <Route path="/people" element={<People />}></Route>
          <Route path="/cars" element={<Cars />}></Route>
          <Route path="/login" element={<Login />}></Route>
          <Route path="/register" element={<Register />}></Route>
          <Route
            path="/politicalCandidates"
            element={<PoliticalCandidates />}
          ></Route>
          <Route path="/basic" element={<Basic />} />
        </Routes>
      </div>

      <Footer></Footer>
    </React.Fragment>
  );
}

export default App;
