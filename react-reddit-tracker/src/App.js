import React, { useState, useEffect } from 'react';
import Header from "./components/Header";
import AddSubredditForm from "./components/AddSubredditForm";
import ButtonAppBar from './components/BasicAppBar';
import SubredditCards from './components/SubredditCards';
import { CreateNewSubredditData, GetSubredditData, UpdateSubredditData, DeleteSubredditData } from './services/api';

function App() {

  const [formToggle, setFormToggle] = useState(false)
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [subredditObjs, setSubredditObjs] = useState([]);

  // load subreddits
  useEffect(() => {
    const fetchData = async () => {
      try {
        const result = await GetSubredditData();
        setSubredditObjs(result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []); // Empty dependency array means this effect runs once when the component mounts

  // if (loading) return <p>Loading...</p>;
  // if (error) return <p>Error: {error}</p>;

  // add Subreddit
  const addSubreddit = async (subredditName) => {
    setLoading(true);
    setError(null);
    try {
      const data = await CreateNewSubredditData(subredditName);
      console.log(data);
      setSubredditObjs([data, ...subredditObjs]);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // update Subreddit
  const updateSubreddit = async (id, newNickname) => {
    console.log('update Subreddit', id)

    setLoading(true);
    setError(null);
    try {
      await UpdateSubredditData(id, newNickname);
      // Update the local state after successful update
      setSubredditObjs(subredditObjs.map((subreddit) =>
        subreddit.id === id ? { ...subreddit, nickname: newNickname } : subreddit
      ));
      
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  // delete Subreddit
  const deleteSubreddit = async (id) => {
    console.log('delete Subreddit', id)

    setLoading(true);
    setError(null);
    try {
      await DeleteSubredditData(id);

      setSubredditObjs(subredditObjs.filter((subredditObj) => subredditObj.id !== id))
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="App">

      <ButtonAppBar  />

      <div className="MainPage" style={{ padding: '2%' }}>
        <Header formToggle={() => setFormToggle(!formToggle)} showHide={formToggle} />
        {formToggle ? <AddSubredditForm onAdd={addSubreddit}/> : ''}
        {subredditObjs.length > 0 ?
         <SubredditCards subredditObjs={subredditObjs} onUpdate={updateSubreddit} onDelete={deleteSubreddit} /> :
         'No Subreddits Added'}
      </div>
    
    </div>
  );
}

export default App;
