package spacetraffic.kiv.zcu.cz.gameelement;

/**
 * ResultState enum.
 */
public enum ResultState {
    SUCCESS,
    FAILURE;

    /**
     * Method for getting state by state in string
     * @param state state in string
     * @return result state or default FAILURE state.
     */
    public static ResultState getStateByName(String state){
        switch (state){
            case "SUCCESS":
                return SUCCESS;
            case "FAILURE":
                return FAILURE;

            default:
                return FAILURE;
        }
    }
}
