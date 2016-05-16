/**
 Copyright 2010 FAV ZCU

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

 **/
package spacetraffic.kiv.zcu.cz.gameelement;

import org.ksoap2.serialization.SoapObject;

/**
 * Result class.
 */
public class Result {

    /**
     * Result state.
     */
    private ResultState state;

    /**
     * Message.
     */
    private String message;

    /**
     * Return value.
     */
    private Object returnValue;

    /**
     * Result constructor.
     * @param object object with result
     */
    public Result(SoapObject object){
        this.message = object.getPropertyAsString("Message");
        this.state = ResultState.getStateByName(object.getPropertyAsString("State"));
        this.returnValue = object.getProperty("ReturnValue");
    }

    /**
     * Getter for result state.
     * @return state
     */
    public ResultState getState() {
        return state;
    }

    /**
     * Getter for message.
     * @return message
     */
    public String getMessage() {
        return message;
    }

    /**
     * Getter fro return value.
     * @return return value
     */
    public Object getReturnValue() {
        return returnValue;
    }

    /**
     * Setter for result state
     * @param state result state
     */
    public void setState(ResultState state) {
        this.state = state;
    }

    /**
     * Setter for message.
     * @param message message
     */
    public void setMessage(String message) {
        this.message = message;
    }

    /**
     * Setter for return value.
     * @param returnValue return value
     */
    public void setReturnValue(Object returnValue) {
        this.returnValue = returnValue;
    }
}
