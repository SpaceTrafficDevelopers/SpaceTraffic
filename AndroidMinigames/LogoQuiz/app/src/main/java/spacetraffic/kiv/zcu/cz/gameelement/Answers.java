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

import android.util.Log;

import org.w3c.dom.Document;
import org.w3c.dom.Element;

import java.io.StringWriter;
import java.util.ArrayList;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

/**
 * ArrayList of Answers.
 */
public class Answers extends ArrayList<Answer> {

    /**
     * Method for creating xml as string from collection.
     * format:
     * <answers>
     *      <answer>
     *          <id>value</id>
     *          <slectedAnswer>value</slectedAnswer>
     *      </answer>
     * </answers>
     * @return xml as string
     */
    public String getXml(){
        try {
            DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
            DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
            Document doc = dBuilder.newDocument();

            Element rootElement = doc.createElement("answers");


            for (Answer answer : this) {
                Element answerElement = doc.createElement("answer");

                Element idElement = doc.createElement("id");
                idElement.setTextContent(Integer.toString(answer.getId()));

                answerElement.appendChild(idElement);

                Element selectedAnswer = doc.createElement("selectedAnswer");
                selectedAnswer.setTextContent(answer.getSelectedAnswer());

                answerElement.appendChild(selectedAnswer);
                rootElement.appendChild(answerElement);
            }

            doc.appendChild(rootElement);

            TransformerFactory tf = TransformerFactory.newInstance();
            Transformer transformer = tf.newTransformer();
            transformer.setOutputProperty(OutputKeys.OMIT_XML_DECLARATION, "yes");
            StringWriter writer = new StringWriter();
            transformer.transform(new DOMSource(doc), new StreamResult(writer));
            return writer.getBuffer().toString();

        } catch (Exception e) {
            Log.e(this.getClass().getSimpleName(), e.getMessage());
        }

        return null;
    }
}
