import * as React from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import CircularProgress from '@mui/material/CircularProgress';
import { debounce } from '@mui/material/utils';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import { getMoviesByTitle } from '../services/movieService';

interface MovieSearchBoxProps {
    onSelectMovie: (movie: Movie | null) => void;
}

const MovieSearchBox: React.FC<MovieSearchBoxProps> = ({ onSelectMovie }) => {
    const [open, setOpen] = React.useState(false);
    const [options, setOptions] = React.useState<readonly Movie[]>([]);
    const [inputValue, setInputValue] = React.useState('');
    const [value, setValue] = React.useState<Movie | null>(null);
    const [loading, setLoading] = React.useState<boolean>(false);

    const debouncedMovieSearchByTitle = React.useMemo(
        () =>
            debounce(
                async (
                    inputValue: string,
                    callback: (results?: readonly Movie[]) => void,
                ) => callback(await getMoviesByTitle(inputValue)),
                400,
            ),
        [],
    );

    React.useEffect(() => {
        let active = true;
        setLoading(true);

        debouncedMovieSearchByTitle(inputValue, (results?: readonly Movie[]) => {
            setLoading(false);
            if (active) {
                setOptions(results as []);
            }
        });

        return () => {
            active = false;
        };
    }, [inputValue, debouncedMovieSearchByTitle]);

    return (
        <Autocomplete
            id="movie-search-box"
            sx={{ width: 400 }}
            autoComplete
            open={open}
            value={value}
            onOpen={() => {
                setOpen(true);
            }}
            onClose={() => {
                setOpen(false);
            }}
            onChange={(_event, newValue: Movie | null) => {
                setValue(newValue);
                onSelectMovie(newValue);
            }}
            onInputChange={(_event, newInputValue) => {
                setInputValue(newInputValue);
            }}
            isOptionEqualToValue={(option, value) => option.title === value.title}
            getOptionLabel={(option) => option.title}
            options={options}
            loading={loading}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label="Search movie by title"
                    InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                            <React.Fragment>
                                {loading ? <CircularProgress color="inherit" size={20} /> : null}
                                {params.InputProps.endAdornment}
                            </React.Fragment>
                        ),
                    }}
                />
            )}
            renderOption={(props, option) => {
                const poster = option.poster === 'N/A'
                    ? <img
                        src="https://www.ncenet.com/wp-content/uploads/2020/04/no-image-png-2.png"
                        width="40px"
                        alt="No poster" />
                    : <img src={option.poster}
                        width="40px"
                        alt="Movie poster" />;

                return (
                    <li {...props}>
                        <Grid container alignItems="center">
                            <Grid item sx={{ display: 'flex', width: 44 }}>
                                {poster}
                            </Grid>
                            <Grid item sx={{ width: 'calc(100% - 44px)', wordWrap: 'break-word' }}>
                                <div data-testid="movie-option-main-text">
                                    {option.title} ({option.year})
                                </div>
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                    data-testid="movie-option-secondary-text">
                                    {option.type}
                                </Typography>
                            </Grid>
                        </Grid>
                    </li>
                );
            }}
        />
    );
}

export default MovieSearchBox;
