import * as React from 'react';
import SubredditCard from './SubredditCard';

const SubredditCards = ({ subredditObjs, onUpdate, onDelete }) => {
  return (

    <>
    {subredditObjs.map((subredditObj) => (
        <SubredditCard key={subredditObj.id} subredditObj={subredditObj} onUpdate={onUpdate} onDelete={onDelete}  >
        </SubredditCard>
      ))}
    </>

  );
}

export default SubredditCards