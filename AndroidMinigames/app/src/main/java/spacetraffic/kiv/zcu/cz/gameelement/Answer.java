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
