import Grid from "@mui/material/Grid";
import * as React from "react";
import { getMovieById } from "../services/movieService";

const MovieDescription: React.FC<{ movie: Movie | null }> = ({ movie }) => {
    const [movieDescription, setMovieDescription] = React.useState<MovieWithDescription | null>(null);

    React.useEffect(() => {
        const getMovieDescription = async (movie: Movie | null) => {
            if (movie && movie.imdbID !== movieDescription?.imdbID) {
                const movieDescription = await getMovieById(movie.imdbID)
                setMovieDescription(movieDescription);
            }
        };

        if (movie == null)
            setMovieDescription(null);
        getMovieDescription(movie);
    }, [movie, movieDescription, setMovieDescription]);

    const movieDescriptionView = !movie
        ? ''
        :
        (
            <Grid container spacing={3}>
                <Grid item xs={6}>
                    <img src={movie?.poster}
                        width="100%"
                        alt="Movie poster" />
                </Grid>
                <Grid item xs={6}>
                    <div>
                        <b>Title: </b>
                        <span data-testid="title">{movie?.title}</span>
                    </div>
                    <div>
                        <b>Year: </b>
                        <span data-testid="year">{movie?.year}</span>
                    </div>
                    <div>
                        <b>Genre: </b>
                        <span data-testid="genre">{movieDescription?.genre}</span>
                    </div>
                    <div>
                        <b>Rating: </b>
                        <span data-testid="rating">{movieDescription?.imdbRating}</span>
                    </div>
                    <div>
                        <b>Country: </b>
                        <span data-testid="country">{movieDescription?.country}</span>
                    </div>
                    <div>
                        <b>Language: </b>
                        <span data-testid="language">{movieDescription?.language}</span>
                    </div>
                    <div>
                        <b>Time: </b>
                        <span data-testid="time">{movieDescription?.runtime}</span>
                    </div>
                    <div>
                        <b>Description: </b>
                        <span data-testid="description">{movieDescription?.plot}</span>
                    </div>
                </Grid>
            </Grid>
        );

    return (
        <React.Fragment>
            {movieDescriptionView}
        </React.Fragment>
    )
}

export default MovieDescription;