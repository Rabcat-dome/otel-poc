import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";

// function fetch() {
//   return new Promise((resolve) => setTimeout(() => resolve(42), 1000));
// }

function fetchAPI() {
  // param is a highlighted word from the user before it clicked the button
  return fetch("http://localhost:3001/Tele").then(function(result){return result});
}

function App() {
  const [count1, setCount1] = useState(0);
  const [count2, setCount2] = useState(0);
  const [txt1, setTxt1] = useState("");

  return (
    <>
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React3</h1>
      <div className="card">
        <button onClick={() => {setCount1((count1) => count1 + 1)
        fetchAPI().then(result => {
          setTxt1((result as string));
        });
        }}>
          call API {count1}
        </button>
        <button onClick={() => setCount2((count2) => count2 + 1)}>
          Call API proxypass {count2}
        </button>
        <p>
          <div>{txt1}</div>
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  );
}

export default App;
