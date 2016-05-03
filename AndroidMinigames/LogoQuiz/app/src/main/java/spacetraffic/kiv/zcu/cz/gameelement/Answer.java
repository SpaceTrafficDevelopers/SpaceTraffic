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

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;

import java.util.Hashtable;

/**
 * Answer class.
 */
public class Answer {

    /**
     * Question id.
     */
    private int id;

    /**
     * Selected answer.
     */
    private String selectedAnswer;

    /**
     * Constructor without parameters.
     */
    public Answer(){}

    /**
     * Answer constructor.
     * @param id id
     * @param selectedAnswer selectedAnswer
     */
    public Answer(int id, String selectedAnswer)
    {
        this.id = id;
        this.selectedAnswer = selectedAnswer;
    }

    /**
     * Getter for id.
     * @return id
     */
    public int getId() {
        return id;
    }

    /**
     * Setter for id.
     * @param id id
     */
    public void setId(int id) {
        this.id = id;
    }

    /**
     * Getter for selected answer.
     * @return selected answer
     */
    public String getSelectedAnswer() {
        return selectedAnswer;
    }

    /**
     * Setter for selected answer.
     * @param selectedAnswer selected answer
     */
    public void setSelectedAnswer(String selectedAnswer) {
        this.selectedAnswer = selectedAnswer;
    }
}
