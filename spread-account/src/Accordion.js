import { React, useState } from "react";
import { animated, useSpring } from "react-spring";

const Accordion = (props) => {
  const [open, setOpen] = useState(false);
  let toggleHandler = (e) => {
    setOpen(!open);
  };
  const openAnimation = useSpring({
    from: { opacity: "0", maxHeight: "130px" },
    to: { opacity: "1", maxHeight: open ? "320px" : "130px" },
    config: { duration: "300" }
  });
  const iconAnimation = useSpring({
    from: {
      transform: "rotate(0deg)",
    },
    to: {
      transform: open ? "rotate(180deg)" : "rotate(0deg)"
    },
    config: { duration: "120" }
  });
  return (
    <animated.div className="accordion__item" style={openAnimation}>
      <div className="accordion__header" onClick={toggleHandler}>
        {props.title}
        <animated.i style={iconAnimation}>
          <i style={{fontSize: '30px'}} className='fas fa-angle-down'></i>
        </animated.i>
      </div>
      <div className="accordion__content">{props.content}</div>
    </animated.div>
  );
}
export default Accordion;