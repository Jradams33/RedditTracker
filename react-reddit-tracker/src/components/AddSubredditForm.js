import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import React, { useState } from 'react';

const AddSubredditForm = ({ onAdd }) => {

    const [subredditText, setSubredditText] = useState('')

    const submitForm = async () => {
        console.log(subredditText)

        if(!subredditText) {
            alert('Please add a subreddit')
            return
        }

        // change this to result from api
        onAdd(subredditText)
    }

    return (

    <Box sx={{ flexGrow: 1 }} style={{ marginBottom: '1em' }}>
        <Grid container spacing={2}>
        <Grid className="showHide" item xs={12} style={{ maxWidth: '50%' }}>
            <TextField id="filled-basic" label="Subreddit Name" variant="filled" fullWidth value={subredditText} onChange={(e) => setSubredditText(e.target.value)}/>
        </Grid>
        <Grid className="showHide" item xs={12}>
            <Button variant="contained" onClick={submitForm}>Save</Button>
        </Grid>
      </Grid>
    </Box>

    )
  }
  
  export default AddSubredditForm