import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';

const Header = ({ formToggle, showHide }) => {

  return (
    <Box sx={{ flexGrow: 1 }}>
      <Grid container spacing={2} style={{ marginBottom: '1em' }}>
        <Grid item xs={4}>
            <Typography variant="h4" component="div" sx={{ flexGrow: 1 }}>
                Subreddits
            </Typography>
        </Grid>
        <Grid item xs={8}>
            <Button variant="contained" onClick={formToggle}>{showHide ? 'Hide' : 'Add'}</Button>
        </Grid>
      </Grid>
    </Box>
  )
}

export default Header
