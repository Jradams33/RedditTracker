import axios from 'axios';

const API_URL = 'https://localhost:7235/api/Subreddit';

export const CreateNewSubredditData = async (subredditName) => {
  try {
    const response = await axios.post(`${API_URL}/CreateNewSubreddit`, {
        subredditName: subredditName
    });

    return response.data;
  } catch (error) {
    // Log and rethrow the error
    console.error('Error fetching subreddit data:', error);
    throw error;
  }
};

export const GetSubredditData = async () => {
    try {
      const response = await axios.get(`${API_URL}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching subreddit data:', error);
      throw error;
    }
};

export const UpdateSubredditData = async (subredditId, subredditNickName) => {
    try {
      const response = await axios.put(`${API_URL}`, {
        subredditId: subredditId,
        subredditNickName: subredditNickName
      });

      return response.data;
    } catch (error) {
      console.error('Error fetching subreddit data:', error);
      throw error;
    }
};

export const DeleteSubredditData = async (subredditId) => {
    try {

      const response = await axios.delete(`${API_URL}/${subredditId}`);
  
      return response.data;
    } catch (error) {
      console.error('Error deleting subreddit data:', error);
      throw error;
    }
  };
