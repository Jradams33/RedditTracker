import React, { useState } from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Link from '@mui/material/Link';


const SubredditCard = ({ subredditObj, onUpdate, onDelete }) => {

  // Local state for nickname
  const [nickname, setNickname] = useState(subredditObj.nickname);

  const handleNicknameChange = (event) => {
    setNickname(event.target.value);
  };

  const handleSaveClick = () => {
    // Pass updated nickname to the parent component
    onUpdate(subredditObj.id, nickname);
  };

  return (

    <Card sx={{ minWidth: 275 }} style={{ marginBottom: '1em', backgroundColor: 'lightgray' }}>
      <CardContent>
        <div style={{ paddingBottom: '1em' }}>
          <Stack spacing={2} style={{ paddingBottom: '1em' }}>
            <TextField id="filled-basic" label="Nickname" variant="standard" style={{ maxWidth: '50%' }} value={nickname} onChange={handleNicknameChange} />
            <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
              <Link href={`https://www.reddit.com/r/${subredditObj.name}`} underline="hover" target="_blank" rel="noopener noreferrer">
                r/{subredditObj.name}
              </Link>
            </Typography>
          </Stack>
        </div>

        <div style={{ paddingBottom: '1em' }}>
          <Typography variant="h6" component="div">
            Top Posts
          </Typography>

          {subredditObj.topics.map((topic) => (
            <Typography key={topic.id} variant="body2" style={{ paddingBottom: '.5em' }}>
              <Link href={topic.url} underline="hover" target="_blank" rel="noopener noreferrer">
                {topic.title}
              </Link>
            </Typography>
          ))}
        </div>
        
        <div>
          <Typography variant="h6" component="div">
            Top Users
          </Typography>

          {subredditObj.topics.map((topic) => (
            <Typography key={topic.id} variant="body2" style={{ paddingBottom: '.5em' }}>
              <Link href={`https://www.reddit.com/user/${topic.author}`} underline="hover" target="_blank" rel="noopener noreferrer">
                {topic.author}
              </Link>
            </Typography>
          ))}
        </div>

      </CardContent>
      <CardActions>
        <Button size="small" variant="contained" onClick={handleSaveClick}>Save Nickname</Button>
        <Button size="small" variant="contained" onClick={() => onDelete(subredditObj.id)} style={{ backgroundColor: "red" }}>Remove</Button>
      </CardActions>
    </Card>

  );
}

export default SubredditCard