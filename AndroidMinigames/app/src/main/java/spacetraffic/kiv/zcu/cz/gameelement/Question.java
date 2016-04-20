package spacetraffic.kiv.zcu.cz.gameelement;

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;

import java.util.ArrayList;
import java.util.Hashtable;
import java.util.List;

/**
 * Question class.
 */
public class Question implements KvmSerializable {

    /**
     * First wrong choice.
     */
    private String firstWrongChoice;

    /**
     * Question id.
     */
    private int id;

    /**
     * Right choice as logo.
     */
    private Logo rightChoice;

    /**
     * Second wrong choice.
     */
    private String secondWrongChoice;

    /**
     * Question constructor.
     * @param soapObject object
     */
    public Question(SoapObject soapObject)
    {
        this.firstWrongChoice = soapObject.getProperty("FirstWrongChoice").toString();
        this.id = Integer.parseInt(soapObject.getProperty("Id").toString());
        this.rightChoice = new Logo((SoapObject)soapObject.getProperty("RightChoice"));
        this.secondWrongChoice = soapObject.getProperty("SecondWrongChoice").toString();
    }

    /**
     * Method for get property.
     * @param arg0 which property
     * @return property
     */
    @Override
    public Object getProperty(int arg0) {
        switch(arg0){
            case 0:
                return firstWrongChoice;
            case 1:
                return id;
            case 2:
                return rightChoice;
            case 3:
                return secondWrongChoice;
        }
        return null;
    }

    /**
     * Method for get property count.
     * @return property count
     */
    @Override
    public int getPropertyCount() {
        return 4;
    }

    /**
     * Method for get property info.
     * @param index index
     * @param arg1 hashtable arguments
     * @param info property info
     */
    @Override
    public void getPropertyInfo(int index, @SuppressWarnings("rawtypes") Hashtable arg1, PropertyInfo info) {
        switch(index){
            case 0:
                info.type = PropertyInfo.STRING_CLASS;
                info.name = "FirstWrongChoice";
                break;
            case 1:
                info.type = PropertyInfo.INTEGER_CLASS;
                info.name = "Id";
                break;
            case 2:
                info.type = Logo.class;
                info.name = "RightChoice";
                break;
            case 3:
                info.type = PropertyInfo.STRING_CLASS;
                info.name = "SecondWrongChoice";
                break;
        }
    }

    /**
     * Setter for property (not used).
     * @param arg0
     * @param arg1
     */
    @Override
    public void setProperty(int arg0, Object arg1) {
    }

    /**
     * Getter for first wrong choice.
     * @return first wrong choice
     */
    public String getFirstWrongChoice() {
        return firstWrongChoice;
    }

    /**
     * Getter for id.
     * @return id
     */
    public int getId() {
        return id;
    }

    /**
     * Getter for right choice.
     * @return right choice
     */
    public Logo getRightChoice() {
        return rightChoice;
    }

    /**
     * Getter for first wrong choice.
     * @return first wrong choice
     */
    public String getSecondWrongChoice() {
        return secondWrongChoice;
    }

    /**
     * Getter for list of questions from object.
     * @param object object
     * @return list of questions
     */
    public static List<Question> getListFromSoapObject (SoapObject object){
        List<Question> questions = new ArrayList<Question>();

        for(int i = 0; i < object.getPropertyCount(); i++){
            SoapObject quest = (SoapObject) object.getProperty(i);
            Question question = new Question(quest);
            questions.add(question);
        }

        return questions;
    }
}
